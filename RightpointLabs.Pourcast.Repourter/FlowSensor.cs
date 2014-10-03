using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using RightpointLabs.Pourcast.Repourter.Configuration;

namespace RightpointLabs.Pourcast.Repourter
{
    public class FlowSensor
    {
        private readonly InterruptPort _port;
        private readonly OutputPort _light;
        private readonly IHttpMessageWriter _httpMessageWriter;
        private readonly string _tapId;
        private readonly PulseConfig _pulseConfig;
        private readonly ILogger _logger;

        private Timer _timer;
        private int _pulseCount = 0;
        private int _lastPulseCount = 0;

        // the goal here is to make sure that FlowDetected *never* has to take a lock, but yet we get a start message as soon as we cross the threshold.
        public FlowSensor(InterruptPort port, OutputPort light, IHttpMessageWriter httpMessageWriter, string tapId, PulseConfig pulseConfig, ILogger logger)
        {
            _port = port;
            _light = light;
            _port.OnInterrupt += FlowDetected;
            _httpMessageWriter = httpMessageWriter;
            _tapId = tapId;
            _pulseConfig = pulseConfig;
            _logger = logger;
            _timer = new Timer(PourCheck, null, _pulseConfig.PourStoppedDelay, _pulseConfig.PourStoppedDelay);
        }

        private void FlowDetected(uint port, uint data, DateTime time)
        {
            Interlocked.Increment(ref _pulseCount);
        }

        private void PourCheck(object state)
        {
            var pulses = _pulseCount; // peek, but don't reset
            if (pulses == 0 && _lastPulseCount == 0)
                return;
            if (_lastPulseCount == 0)
            {
                // something happened, but we aren't in a pour
                if (pulses >= _pulseConfig.MinPulsesRequired)
                {
                    // start the pour
                    _light.Write(true);
                    _httpMessageWriter.SendStartAsync(_tapId);
                    _logger.Log("Started pour " + DateTime.Now.ToString("s"));
                    _lastPulseCount = pulses;
                }
                else
                {
                    // but not enough enough happened to start the pour... let's reset the counter
                    pulses = Interlocked.Exchange(ref _pulseCount, 0);
                    // ok, start message wasn't sent, and the counter is reset - we've sucessfully ignored the pour (even if we got a few more pulses between the peek and the exchange
                    _logger.Log("Ignored pour @ " + pulses + ": " + DateTime.Now.ToString("s"));
                }
            }
            else
            {
                if (pulses - _lastPulseCount < _pulseConfig.PulsesPerStoppedExtension)
                {
                    // we didn't get enough pulses in to continue - complete the pour
                    pulses = Interlocked.Exchange(ref _pulseCount, 0);
                    _logger.Log("Stopped pour @ " + pulses + ": " + DateTime.Now.ToString("s"));
                    _httpMessageWriter.SendStopAsync(_tapId, pulses/_pulseConfig.PulsesPerOunce);
                    _lastPulseCount = 0;
                }
                else
                {
                    // we got enough to keep the pour alive, send the pouring message
                    _logger.Log("Pouring @ " + pulses + ": " + DateTime.Now.ToString("s"));
                    _httpMessageWriter.SendPouringAsync(_tapId, pulses / _pulseConfig.PulsesPerOunce);
                    _lastPulseCount = pulses;
                }
            }
        }
    }
}

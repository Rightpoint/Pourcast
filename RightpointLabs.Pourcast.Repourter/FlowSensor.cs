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

        private Timer _timer;
        private object _lockObject = new object();
        private int _pulseCount = 0;

        public FlowSensor(InterruptPort port, OutputPort light, IHttpMessageWriter httpMessageWriter, string tapId, PulseConfig pulseConfig)
        {
            _port = port;
            _light = light;
            _port.OnInterrupt += FlowDetected;
            _httpMessageWriter = httpMessageWriter;
            _tapId = tapId;
            _pulseConfig = pulseConfig;
        }

        private void FlowDetected(uint port, uint data, DateTime time)
        {
            var pulses = Interlocked.Increment(ref _pulseCount);
            if(pulses == 1)
            {
                lock(_lockObject)
                {
                    _light.Write(true);
                    _httpMessageWriter.SendStartAsync(_tapId);
                    _timer = new Timer(PourCompleted, null, _pulseConfig.PourStoppedDelay, Timeout.Infinite);
                    Debug.Print("Started pour");
                }
            }
            else if (pulses % _pulseConfig.PulsesPerStoppedExtension == 0)
            {
                lock (_lockObject)
                {
                    if (null != _timer)
                    {
                        _timer.Change(_pulseConfig.PourStoppedDelay, Timeout.Infinite);
                        Debug.Print("Extended");
                    }
                }
            }
            else if (pulses % _pulseConfig.PulsesPerPouring == 0)
            {
                lock (_lockObject)
                {
                    if (null != _timer)
                    {
                        _httpMessageWriter.SendPouringAsync(_tapId, pulses / _pulseConfig.PulsesPerOunce);
                    }
                }
            }
        }

        private void PourCompleted(object state)
        {
            lock(_lockObject)
            {
                _timer.Dispose();
                _timer = null;
                var pulses = Interlocked.Exchange(ref _pulseCount, 0);
                _light.Write(false);
                _httpMessageWriter.SendStopAsync(_tapId, pulses / _pulseConfig.PulsesPerOunce);
            }
        }
    }
}

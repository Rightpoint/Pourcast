using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter
{
    public class FlowSensor
    {
        private readonly InterruptPort _port;
        private readonly OutputPort _light;

        private object _lockObject = new object();

        private int _pulseCount = 0;

        private const double PULSES_PER_LITER = 5600.0;

        private const double OUNCES_PER_LITER = 33.814;
        private const int POUR_STOPPED_DELAY = 250;
        private const int PULSES_PER_STOPPED_EXTENSION = 50;

        private readonly string _tapId;

        private readonly IHttpMessageWriter _httpMessageWriter;
        
        private Timer _timer;

        public FlowSensor(InterruptPort port, OutputPort light, IHttpMessageWriter httpMessageWriter, string tapId)
        {
            _port = port;
            _light = light;
            _port.OnInterrupt += FlowDetected;
            _httpMessageWriter = httpMessageWriter;
            _tapId = tapId;
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
                    _timer = new Timer(PourCompleted, null, POUR_STOPPED_DELAY, Timeout.Infinite);
                    Debug.Print("Started pour");
                }
            }
            else if (pulses % PULSES_PER_STOPPED_EXTENSION == 0)
            {
                lock (_lockObject)
                {
                    if (null != _timer)
                    {
                        _timer.Change(POUR_STOPPED_DELAY, Timeout.Infinite);
                        _httpMessageWriter.SendPouringAsync(_tapId, pulses / PULSES_PER_LITER * OUNCES_PER_LITER);
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
                _httpMessageWriter.SendStopAsync(_tapId, pulses / PULSES_PER_LITER * OUNCES_PER_LITER);
            }
        }
    }
}

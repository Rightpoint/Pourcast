using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter
{
    public class FlowSensor
    {
        private readonly InterruptPort _port;

        //private DateTime _flowStart;

        private DateTime _lastFlowDetected;

        private int _pulseCount = 0;

        private double _totalLiters = 0.0;
        
        private const double PULSES_PER_LITER = 5600.0;

        private const double OUNCES_PER_LITER = 33.814;

        private bool _flowing = false;

        private readonly string _tapId;

        private readonly IHttpMessageWriter _httpMessageWriter;

        public FlowSensor(InterruptPort port, IHttpMessageWriter httpMessageWriter, string tapId)
        {
            _port = port;
            _port.OnInterrupt += FlowDetected;
            _httpMessageWriter = httpMessageWriter;
            _tapId = tapId;
        }

        public void CheckPulses()
        {
            // Disable interrupts while we read so that we don't mess with a triggering interrput.
            //_port.DisableInterrupt();
            //var pulses = _pulseCount;
            ////var lastMeasuredTime = _lastFlowDetected;
            //_pulseCount = 0;

            //_port.EnableInterrupt();

            var pulses = Interlocked.Exchange(ref _pulseCount, 0);

            var liters = pulses/PULSES_PER_LITER;
            if (liters > 0) // it actually flowed during this timespan
            {
                _totalLiters += liters;
                if (!_flowing)
                {
                    _flowing = true;
                    //_flowStart = lastMeasuredTime;
                    _httpMessageWriter.SendStartAsync(_tapId);
                }
            }
            else if(_totalLiters > 0) // it didn't flow, but we have finished a pour. Report it.
            {
                var ounces = _totalLiters*OUNCES_PER_LITER;
                _httpMessageWriter.SendStopAsync(_tapId, ounces);
                _totalLiters = 0;
                _flowing = false;
            }
        }

        private void FlowDetected(uint port, uint data, DateTime time)
        {
            var pulses = Interlocked.Increment(ref _pulseCount);
            //_pulseCount++;
            //_lastFlowDetected = time;
            if (pulses % 100 == 0)
                Debug.Print("Flow detected - " + _tapId + ": " + pulses);

        }
    }
}

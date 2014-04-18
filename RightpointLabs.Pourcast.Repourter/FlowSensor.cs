using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter
{
    public class FlowSensor
    {
        private readonly InterruptPort _port;

        private DateTime _flowStart;

        private DateTime _lastFlowDetected;

        private double _pulseCount = 0.0;

        private double _totalLiters = 0.0;
        
        private const int PULSES_PER_LITER = 5600;

        private bool _flowing = false;

        public FlowSensor(InterruptPort port)
        {
            _port = port;
            _port.OnInterrupt += FlowDetected;
        }

        //TODO: Add way to report total flow once flow is finished
        public void CheckPulses()
        {
            // Disable interrupts while we read so that we don't mess with a triggering interrput.
            _port.DisableInterrupt();
            var pulses = _pulseCount;
            var lastMeasuredTime = _lastFlowDetected;
            _pulseCount = 0;

            _port.EnableInterrupt();

            var liters = pulses/PULSES_PER_LITER;
            if (liters > 0) // it actually flowed during this timespan
            {
                _totalLiters += liters;
                if (!_flowing)
                {
                    // TODO: report that flow started
                    _flowing = true;
                    _flowStart = lastMeasuredTime;
                }
            }
            else if(_totalLiters > 0) // it didn't flow, but we have finished a pour. Report it.
            {
                var id = _port.Id;
                //TODO: report total liters, flow start (_flowStart) and flow end (_lastFlowDetected)
                _totalLiters = 0;
            }
        }

        private void FlowDetected(uint port, uint data, DateTime time)
        {
            _pulseCount++;
            _lastFlowDetected = time;
        }
    }
}

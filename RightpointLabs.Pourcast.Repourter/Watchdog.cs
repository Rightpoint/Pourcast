using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter
{
    public class Watchdog : IDisposable
    {
        private ExtendedTimer _timer;
        private TimeSpan _duration;
        public Watchdog(TimeSpan duration)
        {
            _duration = duration;
        }

        public void Start()
        {
            if(null != _timer)
                throw new ArgumentException("Cannot start an already running watchdog");
            _timer = new ExtendedTimer(TriggerReboot, null, _duration, _duration);
        }

        public void Reset()
        {
            if (null == _timer)
                throw new ArgumentException("Cannot reset a non-running watchdog");
            _timer.Change(_duration, _duration);
            Debug.Print("Watchdog reset");
        }

        private void TriggerReboot(object state)
        {
            Debug.Print("Rebooting....");
            Thread.Sleep(200);
            PowerState.RebootDevice(false);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}

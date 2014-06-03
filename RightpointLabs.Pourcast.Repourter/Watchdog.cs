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
        private readonly ThreadStart _triggerAction;

        public Watchdog(TimeSpan duration, ThreadStart triggerAction)
        {
            _duration = duration;
            _triggerAction = triggerAction;
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
            _triggerAction();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }

    public class RebootWatchdog : Watchdog
    {
        public RebootWatchdog(TimeSpan duration) : base(duration, () =>
        {
            Debug.Print("Rebooting...");
            Thread.Sleep(200);
            PowerState.RebootDevice(false);
            Debug.Print("Reboot didn't seem to take... this was after the reboot command....");
        })
        {
        }
    }
}

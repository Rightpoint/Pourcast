using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter
{
    public class Watchdog : IDisposable
    {
        private ExtendedTimer _timer;
        private int _duration;
        private readonly bool _fireOnce;
        private readonly ThreadStart _triggerAction;

        public Watchdog(TimeSpan duration, bool fireOnce, ThreadStart triggerAction)
        {
            _duration = (int)(duration.Ticks / 10000);
            _fireOnce = fireOnce;
            _triggerAction = triggerAction;
        }

        public void Start()
        {
            if(null != _timer)
                throw new ArgumentException("Cannot start an already running watchdog");
            _timer = new ExtendedTimer(TriggerAction, null, _duration, _fireOnce ? Timeout.Infinite : _duration);
        }

        public void Reset()
        {
            if (null == _timer)
                throw new ArgumentException("Cannot reset a non-running watchdog");
            _timer.Change(_duration, _fireOnce ? Timeout.Infinite : _duration);
        }

        private void TriggerAction(object state)
        {
            _triggerAction();
        }

        public void Dispose()
        {
            if (null != _timer)
                _timer.Dispose();
        }
    }

    public class RebootWatchdog : Watchdog
    {
        public RebootWatchdog(TimeSpan duration, ILogger logger) : base(duration, false, () =>
        {
            logger.Log("Watchdown triggering reboot...");
            Thread.Sleep(200);
            PowerState.RebootDevice(false, 1);
            Thread.Sleep(200);
            logger.Log("Reboot didn't seem to take... this was after the reboot command....");
        })
        {
        }
    }
}

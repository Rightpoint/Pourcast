using System;

namespace RightpointLabs.Pourcast.Repourter.Configuration
{
    public class Config
    {
        public ConnectivityConfig Connectivity { get; set; }
        public TapConfig[] Taps { get; set; }
        public TimeSpan WatchdogCheckInterval { get; set; }
    }
}
#if WIFI
using Toolbox.NETMF.Hardware;

namespace RightpointLabs.Pourcast.Repourter.Configuration
{
    public class WifiConfig
    {
        public bool Enabled { get; set; }
        public string SSID { get; set; }
        public string Password { get; set; }
        public WiFlyGSX.AuthMode SecurityMode { get; set; }
        public bool DebugMode { get; set; }
    }
}
#endif
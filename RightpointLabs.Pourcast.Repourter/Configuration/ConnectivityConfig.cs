using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter.Configuration
{
    public class ConnectivityConfig
    {
#if WIFI
        public WifiConfig Wifi { get; set; }
#endif
#if ETHERNET
        public EthernetConfig Ethernet { get; set; }
#endif
        public string BaseUrl { get; set; }
        public Cpu.Pin HttpLight { get; set; }
    }
}
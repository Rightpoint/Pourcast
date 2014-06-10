using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter.Configuration
{
    public class ConnectivityConfig
    {
        public WifiConfig Wifi { get; set; }
        public EthernetConfig Ethernet { get; set; }
        public string BaseUrl { get; set; }
        public Cpu.Pin HttpLight { get; set; }
    }
}
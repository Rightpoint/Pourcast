using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter.Configuration
{
    public class TapConfig
    {
        public Cpu.Pin Input { get; set; }
        public Cpu.Pin Light { get; set; }
        public string TapId { get; set; }
        public PulseConfig PulseConfig { get; set; }
    }
}
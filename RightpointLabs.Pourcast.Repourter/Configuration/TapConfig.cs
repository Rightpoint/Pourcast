using Microsoft.SPOT.Hardware;

namespace RightpointLabs.Pourcast.Repourter.Configuration
{
    public class TapConfig
    {
        public const double SF800_PULSES_PER_OUNCE = 5600.0 / 33.814;

        public int TapNumber { get; set; }
        public double PulsesPerOz { get; set; }
        public Cpu.Pin Light { get; set; }
        public string TapId { get; set; }
    }
}
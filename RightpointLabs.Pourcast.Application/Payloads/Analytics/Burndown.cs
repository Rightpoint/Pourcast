using System;

namespace RightpointLabs.Pourcast.Application.Payloads.Analytics
{
    public class Burndown
    {
        public double PercentRemaining { get; set; }
        public DateTime StartOfBurndown { get; set; }
        public DateTime EndOfBurndown { get; set; }
    }
}
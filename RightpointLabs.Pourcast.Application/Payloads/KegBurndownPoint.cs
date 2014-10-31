using System;

namespace RightpointLabs.Pourcast.Application.Payloads
{
    public class KegBurndownPoint
    {
        public double PercentRemaining { get; private set; }

        public DateTime OccurredOn { get; private set; }

        public KegBurndownPoint(double percentRemaining, DateTime occurredOn)
        {
            this.PercentRemaining = percentRemaining;
            this.OccurredOn = occurredOn;
        }
    }
}

using System;

namespace RightpointLabs.Pourcast.Domain.Events
{
    public class BeerPoured : IDomainEvent
    {
        public string TapId { get; private set; }

        public string KegId { get; private set; }

        public double Volume { get; private set; }

        public DateTime Time { get; private set; }

        public BeerPoured(string tapId, string kegId, double volume, DateTime time)
        {
            TapId = tapId;
            KegId = kegId;
            Volume = volume;
            Time = time;
        }
    }
}

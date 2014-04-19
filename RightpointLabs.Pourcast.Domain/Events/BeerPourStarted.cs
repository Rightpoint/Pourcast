namespace RightpointLabs.Pourcast.Domain.Events
{
    using System;

    public class BeerPourStarted : IDomainEvent
    {
        public string TapId { get; private set; }

        public string KegId { get; private set; }

        public BeerPourStarted(string tapId, string kegId)
        {
            TapId = tapId;
            KegId = kegId;
        }
    }
}
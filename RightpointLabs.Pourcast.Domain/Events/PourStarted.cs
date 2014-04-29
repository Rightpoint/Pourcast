namespace RightpointLabs.Pourcast.Domain.Events
{
    using System;

    public class PourStarted : IDomainEvent
    {
        public string TapId { get; private set; }

        public string KegId { get; private set; }

        public PourStarted(string tapId, string kegId)
        {
            TapId = tapId;
            KegId = kegId;
        }
    }
}
namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegRemoved : IDomainEvent
    {
        public string TapId { get; private set; }

        public string KegId { get; private set; }

        public KegRemoved(string tapId, string kegId)
        {
            TapId = tapId;
            KegId = kegId;
        }
    }
}

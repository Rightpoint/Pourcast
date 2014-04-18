namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegRemovedFromTap : IDomainEvent
    {
        public string TapId { get; private set; }

        public string KegId { get; private set; }

        public KegRemovedFromTap(string tapId, string kegId)
        {
            TapId = tapId;
            KegId = kegId;
        }
    }
}

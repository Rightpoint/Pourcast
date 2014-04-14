namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegTapped : IDomainEvent
    {
        public string TapId { get; private set; }

        public string KegId { get; private set; }

        public KegTapped(string tapId, string kegId)
        {
            TapId = tapId;
            KegId = kegId;
        }
    }
}

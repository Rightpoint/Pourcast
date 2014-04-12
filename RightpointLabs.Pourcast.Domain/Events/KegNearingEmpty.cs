namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegNearingEmpty : IDomainEvent
    {
        public string KegId { get; set; }

        public string TapId { get; set; }

        public KegNearingEmpty(string kegId, string tapId)
        {
            KegId = kegId;
            TapId = tapId;
        }
    }
}

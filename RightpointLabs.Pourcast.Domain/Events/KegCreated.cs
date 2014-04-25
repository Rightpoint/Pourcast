namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegCreated : IDomainEvent
    {
        public string KegId { get; set; }

        public string BeerId { get; set; }

        public KegCreated(string kegId, string beerId)
        {
            KegId = kegId;
            BeerId = beerId;
        }
    }
}

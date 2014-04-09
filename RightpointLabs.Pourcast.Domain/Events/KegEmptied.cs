namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegEmptied : IDomainEvent
    {
        public string KegId { get; set; }

        public string TapId { get; set; }

        public KegEmptied(string kegId, string tapId)
        {
            KegId = kegId;
            TapId = tapId;
        }
    }
}

namespace RightpointLabs.Pourcast.Domain.Events
{
    public class KegEmptied : IDomainEvent
    {
        public string KegId { get; private set; }

        public KegEmptied(string kegId)
        {
            KegId = kegId;
        }
    }
}

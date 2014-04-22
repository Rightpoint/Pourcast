namespace RightpointLabs.Pourcast.Domain.Events
{
    public class TapCreated : IDomainEvent
    {
        public string TapId { get; set; }

        public TapCreated(string tapId)
        {
            TapId = tapId;
        }
    }
}

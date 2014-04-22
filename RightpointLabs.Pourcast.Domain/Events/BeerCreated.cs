namespace RightpointLabs.Pourcast.Domain.Events
{
    public class BeerCreated : IDomainEvent
    {
        public string BeerId { get; set; }

        public BeerCreated(string beerId)
        {
            BeerId = beerId;
        }
    }
}

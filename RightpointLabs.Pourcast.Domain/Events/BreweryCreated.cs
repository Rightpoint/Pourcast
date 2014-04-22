namespace RightpointLabs.Pourcast.Domain.Events
{
    public class BreweryCreated : IDomainEvent
    {
        public string BreweryId { get; set; }

        public BreweryCreated(string breweryId)
        {
            BreweryId = breweryId;
        }
    }
}

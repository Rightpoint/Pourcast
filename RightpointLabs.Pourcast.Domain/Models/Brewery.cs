namespace RightpointLabs.Pourcast.Domain.Models
{
    using RightpointLabs.Pourcast.Domain.Events;

    public class Brewery : Entity
    {
        private Brewery() { }

        public Brewery(string id, string name)
            :base(id)
        {
            Name = name;

            DomainEvents.Raise(new BreweryCreated(id));
        }

        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
    }
}
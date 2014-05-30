namespace RightpointLabs.Pourcast.Domain.Models
{
    using RightpointLabs.Pourcast.Domain.Events;

    public class Beer : Entity
    {
        private Beer() { }

        public Beer(string id, string name)
            : base(id)
        {
            Name = name;
            
            DomainEvents.Raise(new BeerCreated(id));
        }

        public string Name { get; set; }
        public string BreweryId { get; set; }
        public double ABV { get; set; }
        public double BAScore { get; set; }
        public double RPScore { get; set; }
        public string StyleId { get; set; }

        public string Color { get; set; }

        public string Glass { get; set; }

        public string Style { get; set; }
    }
}
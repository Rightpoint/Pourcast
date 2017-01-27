namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Beer : Entity
    {
        private Beer() { }

        public Beer(string id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string BreweryId { get; set; }
        public double ABV { get; set; }
        public double BAScore { get; set; }
        public double RPScore { get; set; }
        public string StyleId { get; set; }
    }
}
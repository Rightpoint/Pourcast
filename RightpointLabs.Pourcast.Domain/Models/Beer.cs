namespace RightpointLabs.Pourcast.Domain.Models
{
    public class Beer : Entity
    {
        private Beer()
        {
        }

        public Beer(string id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Brewery Brewer { get; set; }
        public double ABV { get; set; }
        public int BAScore { get; set; }
        public int RPScore { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Glass { get; set; }
    }
}
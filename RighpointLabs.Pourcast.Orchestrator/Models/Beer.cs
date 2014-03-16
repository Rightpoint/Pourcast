namespace RighpointLabs.Pourcast.Orchestrator.Models
{
    public class Beer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Brewery Brewer { get; set; }
        public double ABV { get; set; }
        public int BAScore { get; set; }
        public int RPScore { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Glass { get; set; }
        public string Slug { get; set; } 
    }
}
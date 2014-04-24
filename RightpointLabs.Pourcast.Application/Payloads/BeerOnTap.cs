namespace RightpointLabs.Pourcast.Application.Payloads
{
    using RightpointLabs.Pourcast.Domain.Models;

    public class BeerOnTap
    {
        public Keg Keg { get; set; }

        public Tap Tap { get; set; }

        public Beer Beer { get; set; }

        public Brewery Brewery { get; set; }
    }
}

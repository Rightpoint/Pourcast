namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    public class Tap
    {
        public Tap(TapName name)
        {
            Name = name;
        }

        public string TapId { get; set; }
        public TapName Name { get; set; }
        public Keg Keg { get; set; }

        public void PourBeer(DateTime dateTime, double volume)
        {
            var pour = new Pour(dateTime, volume);

            Keg.Pours.Add(pour);
        }
    }
}
namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    public class Pour
    {
        public Pour(DateTime pouredDateTime, double volume)
        {
            PouredDateTime = pouredDateTime;
            Volume = volume;
        }

        public DateTime PouredDateTime { get; set; }
        public double Volume { get; set; }
    }
}
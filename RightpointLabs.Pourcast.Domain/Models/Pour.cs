namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    public class Pour : ValueObject
    {
        public Pour(DateTime pouredDateTime, double volume)
        {
            if (volume <= 0) 
                throw new ArgumentOutOfRangeException("volume", "Volume must be greater than zero.");

            PouredDateTime = pouredDateTime;
            Volume = volume;
        }

        public DateTime PouredDateTime { get; set; }
        public double Volume { get; set; }
    }
}
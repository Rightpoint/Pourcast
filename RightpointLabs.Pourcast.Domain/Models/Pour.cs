namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    public class Pour : ValueObject
    {
        public Pour(string kegId, double volume, DateTime pouredDateTime)
        {
            if (volume <= 0) 
                throw new ArgumentOutOfRangeException("volume", "Volume must be greater than zero.");

            KegId = kegId;
            PouredDateTime = pouredDateTime;
            Volume = volume;
        }

        public DateTime PouredDateTime { get; private set; }
        
        public double Volume { get; private set; }

        public string KegId { get; private set; }
    }
}
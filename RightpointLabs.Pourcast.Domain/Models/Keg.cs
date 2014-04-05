namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Keg
    {
        private readonly List<Pour> _pours;

        public Keg(Beer beer, double capacity)
        {
            if (beer == null) throw new ArgumentNullException("beer");
            if (capacity <= 0) throw new ArgumentOutOfRangeException("capacity", "Capacity must be greater than zero.");

            Beer = beer;
            Status = Status.InQueue;
            Capacity = capacity;

            _pours = new List<Pour>();
        }

        public string Id { get; set; }
        public Beer Beer { get; set; }
        public Status Status { get; set; }
        public Tap Tap { get; set; }
        public DateTime? DateTimeTapped { get; set; }
        public DateTime? DateTimeEmptied { get; set; }

        public IEnumerable<Pour> Pours
        {
            get
            {
                return _pours;
            }
        }

        public double Capacity { get; set; }

        public double AmountOfBeerPoured
        {
            get
            {
                return Pours.Sum(x => x.Volume);
            }
        }

        public double AmountOfBeerRemaining
        {
            get
            {
                return Capacity - AmountOfBeerPoured;
            }
        }

        public double PercentRemaining
        {
            get
            {
                return AmountOfBeerRemaining / Capacity;
            }
        }

        public void PourBeer(DateTime pouredDateTime, double volume)
        {
            if (AmountOfBeerRemaining - volume < 0) 
                throw new Exception("Volume exceeds amount of beer remaining.");

            var newPour = new Pour(pouredDateTime, volume);
            _pours.Add(newPour);
        }
    }
}
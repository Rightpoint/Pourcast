namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Keg
    {
        private List<Pour> _pours;

        public Keg(Beer beer, Status status, double capacity)
        {
            Beer = beer;
            Status = status;
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
            var newPour = new Pour(pouredDateTime, volume);
            _pours.Add(newPour);
        }
    }
}
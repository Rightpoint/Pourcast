namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Keg
    {
        public Keg(Beer beer, Status status, double capacity)
        {
            Beer = beer;
            Status = status;
            Capacity = capacity;

            Pours = new List<Pour>();
        }

        public string Id { get; set; }
        public Beer Beer { get; set; }
        public Status Status { get; set; }
        public Tap Tap { get; set; }
        public DateTime? TapDateTime { get; set; }
        public IEnumerable<Pour> Pours { get; set; }
        public double Capacity { get; set; }

        public double BeerPoured
        {
            get
            {
                return Pours.Sum(x => x.Volume);
            }
        }

        public double BeerRemaining
        {
            get
            {
                return Capacity - BeerPoured;
            }
        }

        public double PercentRemaining
        {
            get
            {
                return BeerRemaining / Capacity;
            }
        }
    }
}
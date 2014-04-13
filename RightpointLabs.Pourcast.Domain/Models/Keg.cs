namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    using RightpointLabs.Pourcast.Domain.Events;

    public class Keg : Entity
    {
        public Keg(string id, string beerId, double capacity)
            : base(id)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException("capacity", "Capacity must be greater than zero.");

            Capacity = capacity;
            BeerId = beerId;
        }

        public string BeerId { get; private set; }

        public double Capacity { get; private set; }

        public double AmountOfBeerPoured { get; private set; }

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

        public bool IsEmpty
        {
            get
            {
                return AmountOfBeerRemaining <= 0;
            }
        }

        public void PourBeerFromTap(string tapId, double volume)
        {
            if (volume <= 0)
                throw new ArgumentOutOfRangeException("volume", "Volume must be a positive number.");
            
            AmountOfBeerPoured += volume;

            DomainEvents.Raise(new BeerPoured(tapId, Id, volume));

            if (IsEmpty)
            {
                DomainEvents.Raise(new KegEmptied(Id));
            }
        }
    }
}
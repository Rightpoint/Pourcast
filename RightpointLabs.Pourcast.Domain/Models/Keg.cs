namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    using RightpointLabs.Pourcast.Domain.Events;

    public class Keg : Entity
    {
        private Keg() { }

        public Keg(string id, string beerId, double capacity)
            : base(id)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException("capacity", "Capacity must be greater than zero.");

            Capacity = capacity;
            BeerId = beerId;
            IsPouring = false;

            DomainEvents.Raise(new KegCreated(id, beerId));
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

        public bool IsPouring { get; private set; }

        public void StartPourFromTap(string tapId)
        {
            if (IsPouring)
                throw new Exception("Keg is already pouring.");

            IsPouring = true;

            DomainEvents.Raise(new BeerPourStarted(tapId, Id));
        }

        public void StopPourFromTap(string tapId, double volume)
        {
            if (!IsPouring)
                throw new Exception("Keg isn't currently pouring.");

            if (volume <= 0)
                throw new ArgumentOutOfRangeException("volume", "Volume must be a positive number.");

            if (AmountOfBeerRemaining <= 0) return;
            
            AmountOfBeerPoured += volume;
            IsPouring = false;

            DomainEvents.Raise(new BeerPourStopped(tapId, Id, volume, PercentRemaining));

            if (IsEmpty)
            {
                DomainEvents.Raise(new KegEmptied(Id));
            }
        }
    }
}
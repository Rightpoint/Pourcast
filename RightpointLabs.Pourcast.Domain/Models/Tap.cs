namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Events;

    public class Tap : Entity
    {
        private readonly List<Pour> _pours;

        public Tap(string id, TapName name)
            : base(id)
        {
            Name = name;

            _pours = new List<Pour>();
        }

        public TapName Name { get; set; }

        public string KegId { get; set; }

        public IEnumerable<Pour> Pours
        {
            get
            {
                return _pours;
            }
        }

        public void PoorBeer(double volume, DateTime time)
        {
            var pour = new Pour(KegId, volume, time);
            _pours.Add(pour);

            DomainEvents.Raise(new BeerPoured(Id, KegId, volume, time));
        }
    }
}
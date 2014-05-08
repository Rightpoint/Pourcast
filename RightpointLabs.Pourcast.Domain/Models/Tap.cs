namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    using RightpointLabs.Pourcast.Domain.Events;

    public class Tap : Entity
    {
        private Tap() { }

        public Tap(string id, string name)
            : base(id)
        {
            Name = name;

            DomainEvents.Raise(new TapCreated(id));
        }

        public string Name { get; private set; }

        public string KegId { get; private set; }

        public bool HasKeg { get; private set; }

        public void RemoveKeg()
        {
            if (!HasKeg) return;

            var kegId = KegId;
            KegId = null;
            HasKeg = false;

            DomainEvents.Raise(new KegRemovedFromTap(Id, kegId));
        }

        public void TapKeg(string kegId)
        {
            if (string.IsNullOrWhiteSpace(kegId)) throw new ArgumentNullException("kegId");

            if (HasKeg)
                throw new Exception("Tap already has a keg.");

            KegId = kegId;
            HasKeg = true;

            DomainEvents.Raise(new KegTapped(Id, KegId));
        }
    }
}
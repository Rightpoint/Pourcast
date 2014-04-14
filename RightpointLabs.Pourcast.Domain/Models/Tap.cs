namespace RightpointLabs.Pourcast.Domain.Models
{
    using System;

    using RightpointLabs.Pourcast.Domain.Events;

    public class Tap : Entity
    {
        public Tap(string id, TapName name)
            : base(id)
        {
            Name = name;
        }

        public TapName Name { get; private set; }

        public string KegId { get; private set; }

        public bool HasKeg
        {
            get
            {
                return KegId != null;
            }
        }

        public void RemoveKeg()
        {
            if (!HasKeg) return;

            var kegId = KegId;
            KegId = null;

            DomainEvents.Raise(new KegRemovedFromTap(Id, kegId));
        }

        public void TapKeg(string kegId)
        {
            if (HasKeg)
                throw new Exception("Tap already has a keg.");

            KegId = kegId;

            DomainEvents.Raise(new KegTapped(Id, KegId));
        }
    }
}
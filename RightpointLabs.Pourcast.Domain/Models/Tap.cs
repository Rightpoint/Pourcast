namespace RightpointLabs.Pourcast.Domain.Models
{
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

        public void RemoveKeg()
        {
            var kegId = KegId;
            KegId = null;

            DomainEvents.Raise(new KegRemovedFromTap(Id, kegId));
        }

        public void TapKeg(string kegId)
        {
            KegId = kegId;

            DomainEvents.Raise(new KegTapped(Id, KegId));
        }
    }
}
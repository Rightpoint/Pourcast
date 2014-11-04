namespace RightpointLabs.Pourcast.Domain.Events
{
    using System;

    using RightpointLabs.Pourcast.Domain.Models;

    public class StoredEvent : Entity
    {
        public DateTime OccuredOn { get; private set; }

        public IDomainEvent DomainEvent { get; private set; }

        public string TypeName { get; private set; }

        public StoredEvent(string id, DateTime occuredOn, IDomainEvent domainEvent)
            : base(id)
        {
            OccuredOn = occuredOn;
            DomainEvent = domainEvent;
            TypeName = domainEvent.GetType().Name;
        }
    }
}

namespace RightpointLabs.Pourcast.Domain.Events
{
    using System;

    using RightpointLabs.Pourcast.Domain.Models;

    public class StoredEvent<T> : Entity where T : IDomainEvent
    {
        public DateTime OccuredOn { get; private set; }

        public T DomainEvent { get; private set; }

        public string TypeName { get; private set; }

        public StoredEvent(string id, DateTime occuredOn, T domainEvent)
            : base(id)
        {
            OccuredOn = occuredOn;
            DomainEvent = domainEvent;
            TypeName = typeof(T).Name;
        }
    }
}

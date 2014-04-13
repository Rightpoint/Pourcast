namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System;
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Events;

    public interface IStoredEventRepository
    {
        void Add(StoredEvent domainEvent);
        
        IEnumerable<StoredEvent> GetAll();

        StoredEvent GetById(string id);

        string NextIdentity();

        IEnumerable<StoredEvent> GetAll<T>() where T : IDomainEvent;

        IEnumerable<StoredEvent> Find<T>(Func<StoredEvent, bool> storedEventPredicate, Func<T, bool> domainEventPredicate) where T : IDomainEvent;
    }
}

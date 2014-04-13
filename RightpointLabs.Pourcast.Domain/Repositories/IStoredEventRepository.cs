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

        IEnumerable<StoredEvent> Find(Func<StoredEvent, bool> predicate);

        IEnumerable<StoredEvent> Find<T>(Func<StoredEvent, T, bool> predicate) where T : IDomainEvent;
    }
}

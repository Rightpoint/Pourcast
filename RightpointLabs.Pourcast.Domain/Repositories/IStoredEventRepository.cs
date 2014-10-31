namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System;
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Events;

    public interface IStoredEventRepository<T> where T : IDomainEvent
    {
        void Add(StoredEvent<IDomainEvent> storedEvent);

        StoredEvent<T> GetById(string id);

        string NextIdentity();

        IEnumerable<StoredEvent<T>> GetAll();

        IEnumerable<StoredEvent<T>> Find(Func<StoredEvent<T>, bool> predicate);
    }
}

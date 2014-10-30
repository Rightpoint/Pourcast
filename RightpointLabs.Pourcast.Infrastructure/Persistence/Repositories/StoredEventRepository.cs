namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

    public class StoredEventRepository : EntityRepository<StoredEvent>, IStoredEventRepository
    {
        public StoredEventRepository(StoredEventCollectionDefinition storedEventCollectionDefinition)
            : base(storedEventCollectionDefinition)
        {
        }

        public IEnumerable<StoredEvent> GetAll<T>() where T : class, IDomainEvent
        {
            return Queryable.Where(e => e.DomainEvent.GetType() == typeof(T)).AsEnumerable();
        }

        public IEnumerable<StoredEvent> Find(Func<StoredEvent, bool> predicate)
        {
            return Queryable.Where(predicate);
        }

        public IEnumerable<StoredEvent> Find<T>(Func<StoredEvent, bool> predicate) where T : class, IDomainEvent
        {
            return Queryable.Where(e => e.DomainEvent.GetType() == typeof(T)).Where(predicate).AsEnumerable();
        }
    }
}
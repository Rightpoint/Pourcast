using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

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
            var q = Query<StoredEvent>.Where(e => e.DomainEvent.GetType() == typeof(T));
            Debug.WriteLine(q.ToJson());
            var exp = Collection.FindAs<StoredEvent>(q).Explain();
            Debug.WriteLine(exp.ToJson());
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

        public IEnumerable<StoredEvent> GetLatest(int limit)
        {
            return Queryable.OrderByDescending(e => e.OccuredOn).Take(limit).AsEnumerable();
        }
    }
}
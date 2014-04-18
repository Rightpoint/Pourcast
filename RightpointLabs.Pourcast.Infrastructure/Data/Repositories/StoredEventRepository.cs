namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class StoredEventRepository : EntityRepository<StoredEvent>, IStoredEventRepository
    {
        static StoredEventRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(StoredEvent)))
            {
                BsonClassMap.RegisterClassMap<StoredEvent>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }

        public StoredEventRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public IEnumerable<StoredEvent> GetAll<T>() where T : class, IDomainEvent
        {
            return Queryable.Where(e => e.DomainEvent is T).AsEnumerable();
        }

        public IEnumerable<StoredEvent> Find(Func<StoredEvent, bool> predicate)
        {
            return Queryable.Where(predicate);
        }

        public IEnumerable<StoredEvent> Find<T>(Func<StoredEvent, bool> predicate) where T : class, IDomainEvent
        {
            return Queryable.Where(e => e.DomainEvent is T).Where(predicate).AsEnumerable();
        }
    }
}
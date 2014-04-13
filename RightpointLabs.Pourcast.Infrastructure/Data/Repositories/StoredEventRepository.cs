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
            BsonClassMap.RegisterClassMap<StoredEvent>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapCreator(e => new StoredEvent(e.Id, e.OccuredOn, e.DomainEvent));
                });
        }

        public StoredEventRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public IEnumerable<StoredEvent> GetAll<T>() where T : IDomainEvent
        {
            return Queryable.Where(e => e.DomainEvent is T).AsEnumerable();
        }

        public IEnumerable<StoredEvent> Find(Func<StoredEvent, bool> predicate)
        {
            return Queryable.Where(predicate);
        }

        public IEnumerable<StoredEvent> Find<T>(Func<StoredEvent, T, bool> predicate) where T : IDomainEvent
        {
            return Queryable.Where(e => e.DomainEvent is T).Where(e => predicate(e, (T)e.DomainEvent)).AsEnumerable();
        }
    }
}
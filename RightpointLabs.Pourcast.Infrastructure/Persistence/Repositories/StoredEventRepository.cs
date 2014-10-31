namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

    public class StoredEventRepository<T> : EntityRepository<StoredEvent<IDomainEvent>>, IStoredEventRepository<T> where T : IDomainEvent
    {
        private MongoCollection<StoredEvent<IDomainEvent>> _collection;

        private new IQueryable<StoredEvent<T>> Queryable
        {
            get
            {
                return _collection.AsQueryable<StoredEvent<T>>().Where(x => x.DomainEvent.GetType() == typeof(T));
            }
        }

        public StoredEventRepository(StoredEventCollectionDefinition storedEventCollectionDefinition)
            : base(storedEventCollectionDefinition)
        {
            _collection = storedEventCollectionDefinition.Collection;
        }

        public new StoredEvent<T> GetById(string id)
        {
            return Queryable.Single(x => x.Id == id);
        }

        public new IEnumerable<StoredEvent<T>> GetAll()
        {
            return Queryable;
        }

        public IEnumerable<StoredEvent<T>> Find(Func<StoredEvent<T>, bool> predicate)
        {
            return Queryable.Where(predicate);
        }
    }
}
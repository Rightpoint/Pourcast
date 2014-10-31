namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

    public abstract class EntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        protected readonly MongoCollection<T> Collection;

        protected virtual IQueryable<T> Queryable
        {
            get
            {
                return Collection.AsQueryable();
            }
            
        }

        protected EntityRepository(EntityCollectionDefinition<T> collectionDefinition)
        {
            Collection = collectionDefinition.Collection;
        } 

        public virtual void Add(T entity)
        {
            var result = Collection.Save(entity, new MongoInsertOptions
                {
                    WriteConcern = WriteConcern.Acknowledged
                });

            if (!result.Ok)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public virtual void Delete(string id)
        {
            var result = Collection.Remove(Query<T>.EQ(e => e.Id, id), RemoveFlags.None, WriteConcern.Acknowledged);
            if (!result.Ok)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public virtual T GetById(string id)
        {
            return Queryable.SingleOrDefault(e => e.Id == id);
        }

        public virtual IEnumerable<T> GetByIds(IEnumerable<string> ids)
        {
            return Queryable.Where(e => ids.Contains(e.Id));
        }

        public virtual void Update(T entity)
        {
            var result = Collection.Save(entity, WriteConcern.Acknowledged);

            if (!result.Ok)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Queryable.AsEnumerable();
        }

        public virtual string NextIdentity()
        {
            return ObjectId.GenerateNewId(DateTime.Now).ToString();
        }
    }
}
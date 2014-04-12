namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.IdGenerators;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    using RightpointLabs.Pourcast.Domain.Models;

    public abstract class EntityRepository<TModel> : IEntityRepository<TModel> where TModel : Entity
    {
        protected readonly IMongoConnectionHandler<TModel> MongoConnectionHandler;

        static EntityRepository()
        {
            BsonClassMap.RegisterClassMap<Entity>(
                cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
                });
        } 

        protected EntityRepository(IMongoConnectionHandler<TModel> connectionHandler)
        {
            MongoConnectionHandler = connectionHandler;
        } 

        public virtual void Add(TModel entity)
        {
            var result = MongoConnectionHandler.MongoCollection.Save(entity, new MongoInsertOptions
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
            var result = MongoConnectionHandler.MongoCollection.Remove(Query<TModel>.EQ(e => e.Id, id),
                                                                       RemoveFlags.None, WriteConcern.Acknowledged);
            if (!result.Ok)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public virtual TModel GetById(string id)
        {
            var query = Query<TModel>.EQ(e => e.Id, id);
            var entity = this.MongoConnectionHandler.MongoCollection.FindOne(query);

            return entity;
        }

        public virtual void Update(TModel entity)
        {
            var result = MongoConnectionHandler.MongoCollection.Save(entity, WriteConcern.Acknowledged);

            if (!result.Ok)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public virtual IEnumerable<TModel> GetAll()
        {
            return MongoConnectionHandler.MongoCollection.FindAllAs<TModel>().AsEnumerable();
        }

        public virtual string NextIdentity()
        {
            return new ObjectId().ToString();
        }
    }
}
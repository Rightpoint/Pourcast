using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Repository.Abstract
{
    public abstract class EntityRepository<T> : IEntityRepository<T> where T : IMongoEntity
    {
        protected readonly IMongoConnectionHandler<T> MongoConnectionHandler; 
        protected EntityRepository(IMongoConnectionHandler<T> connectionHandler)
        {
            MongoConnectionHandler = connectionHandler;
        } 

        public virtual void Create(T entity)
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
            var result = MongoConnectionHandler.MongoCollection.Remove(Query<T>.EQ(e => e.Id, new ObjectId(id)),
                                                                       RemoveFlags.None, WriteConcern.Acknowledged);
            if (!result.Ok)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public virtual T GetById(string id)
        {
            var query = Query<T>.EQ(e => e.Id, new ObjectId(id));
            return this.MongoConnectionHandler.MongoCollection.FindOne(query);
        }

        public abstract void Update(T entity);

    }
}
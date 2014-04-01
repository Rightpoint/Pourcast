namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    using RightpointLabs.Pourcast.Infrastructure.Data.Entities;

    public abstract class EntityRepository<TModel, TEntity> : IEntityRepository<TModel> where TEntity : IMongoEntity
    {
        protected readonly IMongoConnectionHandler<TEntity> MongoConnectionHandler; 
        protected EntityRepository(IMongoConnectionHandler<TEntity> connectionHandler)
        {
            MongoConnectionHandler = connectionHandler;
        } 

        public virtual void Create(TModel entity)
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
            var result = MongoConnectionHandler.MongoCollection.Remove(Query<TEntity>.EQ(e => e.Id, new ObjectId(id)),
                                                                       RemoveFlags.None, WriteConcern.Acknowledged);
            if (!result.Ok)
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        public virtual TModel GetById(string id)
        {
            var query = Query<TEntity>.EQ(e => e.Id, new ObjectId(id));
            var entity = this.MongoConnectionHandler.MongoCollection.FindOne(query);

            return Mapper.Map<TEntity, TModel>(entity);
        }

        public abstract void Update(TModel entity);

        public IEnumerable<TModel> GetAll()
        {
            var result = MongoConnectionHandler.MongoCollection.FindAllAs<TEntity>().AsEnumerable();

            return Mapper.Map<IEnumerable<TEntity>, IEnumerable<TModel>>(result);
        }
    }
}
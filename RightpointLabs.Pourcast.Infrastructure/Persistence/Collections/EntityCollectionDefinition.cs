namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Collections
{
    using System;

    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.IdGenerators;
    using MongoDB.Driver;

    using RightpointLabs.Pourcast.Domain.Models;

    public abstract class EntityCollectionDefinition<T> where T : Entity
    {
        protected EntityCollectionDefinition(IMongoConnectionHandler connectionHandler)
        {
            if (connectionHandler == null) throw new ArgumentNullException("connectionHandler");

            Collection = connectionHandler.Database.GetCollection<T>(typeof(T).Name.ToLower() + "s");

            // setup serialization
            if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
            {
                BsonClassMap.RegisterClassMap<Entity>(
                cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                    cm.IdMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
                });
            }
        }

        public readonly MongoCollection<T> Collection;
    }
}

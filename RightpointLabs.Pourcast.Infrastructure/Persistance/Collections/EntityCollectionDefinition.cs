using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Collections
{
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

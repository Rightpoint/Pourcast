using System;

namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System.Linq;

    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class TapRepository : EntityRepository<Tap>, ITapRepository
    {
        static TapRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Tap)))
            {
                BsonClassMap.RegisterClassMap<Tap>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }

        public TapRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public Tap GetByKegId(string kegId)
        {
            return Queryable.Single(t => t.KegId == kegId);
        }

        public Tap GetByName(string name)
        {
            try
            {
                return Queryable.Single(t => t.Name == name);
            }
            catch(Exception ex)
            {
                // Probably blew up cause it didn't find anything. 
                // Not sure why mongodb doesn't just return null
                return null;
            }
        }
    }
}
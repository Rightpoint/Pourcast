using System.Linq;

namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class BreweryRepository : EntityRepository<Brewery>, IBreweryRepository
    {
        static BreweryRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Brewery)))
            {
                BsonClassMap.RegisterClassMap<Brewery>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }

        public BreweryRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }


        public Brewery GetByName(string name)
        {
            return Queryable.Single(e => e.Name == name);
        }
    }
}
namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerRepository : EntityRepository<Beer>, IBeerRepository
    {
        static BeerRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Beer)))
            {
                BsonClassMap.RegisterClassMap<Beer>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }

        public BeerRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class KegRepository : EntityRepository<Keg>,  IKegRepository
    {
        static KegRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Keg)))
            {
                BsonClassMap.RegisterClassMap<Keg>(
                cm =>
                {
                    cm.AutoMap();
                });
            }
        }

        public KegRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
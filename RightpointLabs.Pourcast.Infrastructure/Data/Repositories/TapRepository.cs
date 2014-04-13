namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class TapRepository : EntityRepository<Tap>, ITapRepository
    {
        static TapRepository()
        {
            BsonClassMap.RegisterClassMap<Tap>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapCreator(t => new Tap(t.Id, t.Name));
                });
        }

        public TapRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
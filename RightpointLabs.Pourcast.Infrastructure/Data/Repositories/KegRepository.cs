namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Bson.Serialization;
    using MongoDB.Driver.Builders;

    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class KegRepository : EntityRepository<Keg>,  IKegRepository
    {
        static KegRepository()
        {
            BsonClassMap.RegisterClassMap<Keg>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapCreator(k => new Keg(k.Id, k.BeerId, k.Capacity));
                });
        }

        public KegRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
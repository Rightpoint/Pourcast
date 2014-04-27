using System;

namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System.Linq;
    using MongoDB.Bson.Serialization;
    using System.Collections.Generic;
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

        public IEnumerable<Keg> GetAll(bool isEmpty)
        {
            return isEmpty ? Queryable.Where(k => (k.Capacity - k.AmountOfBeerPoured).Equals(0)) : Queryable.Where(k => (k.Capacity - k.AmountOfBeerPoured) > 0);
        }
    }
}
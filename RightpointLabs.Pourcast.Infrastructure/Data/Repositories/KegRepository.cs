namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using MongoDB.Driver.Builders;

    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class KegRepository : EntityRepository<Keg, Entities.Keg>,  IKegRepository
    {
        public KegRepository(IMongoConnectionHandler<Entities.Keg> connectionHandler)
            : base(connectionHandler)
        {
            Mapper.CreateMap<Entities.Keg, Keg>()
                .ConstructUsing(k => new Keg(k.Capacity));

            Mapper.CreateMap<Keg, Entities.Keg>();
        }

        public IEnumerable<Keg> OnTap()
        {
            var query = Query<Entities.Keg>.NotIn(e => e.Tap.Id, new [] { "0" });
            var kegs = MongoConnectionHandler.MongoCollection.FindAs<Entities.Keg>(query).AsEnumerable();

            return Mapper.Map<IEnumerable<Entities.Keg>, IEnumerable<Keg>>(kegs);
        }

        public Keg OnTap(string tapId)
        {
            var query = Query<Entities.Keg>.EQ(x => x.Tap.Id, tapId);
            var keg = MongoConnectionHandler.MongoCollection.FindOne(query);

            return Mapper.Map<Entities.Keg, Keg>(keg);
        }
    }
}
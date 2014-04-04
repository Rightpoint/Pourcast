namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class KegRepository : EntityRepository<Keg, Entities.Keg>,  IKegRepository
    {
        public KegRepository(IMongoConnectionHandler<Entities.Keg> connectionHandler)
            : base(connectionHandler)
        {
        }

        public IEnumerable<Keg> OnTap()
        {
            var taps = MongoConnectionHandler.MongoCollection.FindAllAs<Entities.Tap>().AsEnumerable();
            var kegs = taps.Select(x => x.Keg);

            return Mapper.Map<IEnumerable<Entities.Keg>, IEnumerable<Keg>>(kegs);
        }

        public Keg OnTap(string tapId)
        {
            var query = Query<Entities.Tap>.EQ(x => x.Id, new ObjectId(tapId));
            var tap = MongoConnectionHandler.MongoCollection.FindOneAs<Entities.Tap>(query);
            var keg = tap.Keg;

            return Mapper.Map<Entities.Keg, Keg>(keg);
        }

        public override void Update(Keg entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
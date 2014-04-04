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
        }

        public IEnumerable<Keg> OnTap()
        {
            var query = Query<Entities.Keg>.NotIn(e => e.Tap.TapId, new[] {0});
            var kegs = MongoConnectionHandler.MongoCollection.FindAs<Entities.Keg>(query).AsEnumerable();

            return Mapper.Map<IEnumerable<Entities.Keg>, IEnumerable<Keg>>(kegs);
        }

        public Keg OnTap(int tapId)
        {
            var query = Query<Entities.Keg>.EQ(x => x.Tap.TapId, tapId);
            var keg = MongoConnectionHandler.MongoCollection.FindOne(query);

            return Mapper.Map<Entities.Keg, Keg>(keg);
        }

        public override void Update(Keg entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
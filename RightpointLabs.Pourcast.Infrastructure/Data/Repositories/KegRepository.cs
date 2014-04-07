namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Driver.Builders;

    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class KegRepository : EntityRepository<Keg>,  IKegRepository
    {
        public KegRepository(IMongoConnectionHandler<Keg> connectionHandler, MongoClassMapper mapper)
            : base(connectionHandler, mapper)
        {
        }

        public IEnumerable<Keg> OnTap()
        {
            var query = Query<Keg>.NotIn(e => e.Tap.Id, new [] { "0" });
            var kegs = MongoConnectionHandler.MongoCollection.FindAs<Keg>(query).AsEnumerable();

            return kegs;
        }

        public Keg OnTap(string tapId)
        {
            var query = Query<Keg>.EQ(x => x.Tap.Id, tapId);
            var keg = MongoConnectionHandler.MongoCollection.FindOne(query);

            return keg;
        }
    }
}
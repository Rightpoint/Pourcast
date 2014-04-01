namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

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
            var query = Query<Keg>.NotIn(e => e.Tap.TapId, new[] {0});
            return MongoConnectionHandler.MongoCollection.FindAs<Keg>(query).AsEnumerable();
        }

        public override void Update(Keg entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
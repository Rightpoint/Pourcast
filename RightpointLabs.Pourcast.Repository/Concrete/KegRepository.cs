using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.Repository.Abstract;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Repository.Concrete
{
    public class KegRepository : EntityRepository<Keg>,  IKegRepository
    {
        public KegRepository(IMongoConnectionHandler<Keg> connectionHandler) : base(connectionHandler)
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
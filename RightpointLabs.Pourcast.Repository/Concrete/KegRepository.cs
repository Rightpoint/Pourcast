using System.Collections.Generic;
using System.Linq;
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

        public List<Keg> GetAll()
        {
            return MongoConnectionHandler.MongoCollection.FindAllAs<Keg>().ToList();
        }

        public override void Update(Keg entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
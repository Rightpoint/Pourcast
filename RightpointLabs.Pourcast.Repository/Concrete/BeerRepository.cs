using MongoDB.Driver.Linq;
using System.Linq;
using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.Repository.Abstract;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Repository.Concrete
{
    public class BeerRepository : EntityRepository<Beer>, IBeerRepository
    {
        public BeerRepository(IMongoConnectionHandler<Beer> connectionHandler) : base(connectionHandler)
        {
        }

        public override void Update(Beer entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
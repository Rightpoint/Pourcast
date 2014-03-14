using RightpointLabs.Pourcast.DataModel;
using RightpointLabs.Pourcast.DataModel.Entities;
using RightpointLabs.Pourcast.Repository.Abstract;

namespace RightpointLabs.Pourcast.Repository.Concrete
{
    public class BreweryRepository : EntityRepository<Brewery>, IBreweryRepository
    {
        public BreweryRepository(IMongoConnectionHandler<Brewery> connectionHandler) : base(connectionHandler)
        {
        }

        public override void Update(Brewery entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
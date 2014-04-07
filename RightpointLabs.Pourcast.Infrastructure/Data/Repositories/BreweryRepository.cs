namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Models;

    public class BreweryRepository : EntityRepository<Brewery>, IBreweryRepository
    {
        public BreweryRepository(IMongoConnectionHandler<Brewery> connectionHandler, MongoClassMapper mapper)
            : base(connectionHandler, mapper)
        {
        }
    }
}
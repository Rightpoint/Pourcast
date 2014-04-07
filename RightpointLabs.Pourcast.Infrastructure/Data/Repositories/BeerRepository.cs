namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerRepository : EntityRepository<Beer>, IBeerRepository
    {
        public BeerRepository(IMongoConnectionHandler<Beer> connectionHandler, MongoClassMapper mapper)
            : base(connectionHandler, mapper)
        {
        }
    }
}
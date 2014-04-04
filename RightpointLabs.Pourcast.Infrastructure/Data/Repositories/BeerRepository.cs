namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerRepository : EntityRepository<Beer, Entities.Beer>, IBeerRepository
    {
        public BeerRepository(IMongoConnectionHandler<Entities.Beer> connectionHandler)
            : base(connectionHandler)
        {
        }

        public override void Update(Beer entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
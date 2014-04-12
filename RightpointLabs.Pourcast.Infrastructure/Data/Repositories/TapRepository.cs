namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class TapRepository : EntityRepository<Tap>, ITapRepository
    {
        public TapRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
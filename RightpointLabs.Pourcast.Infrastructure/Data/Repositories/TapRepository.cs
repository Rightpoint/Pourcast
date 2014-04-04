using System;

namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class TapRepository : EntityRepository<Tap, Entities.Tap>, ITapRepository 
    {
        public TapRepository(IMongoConnectionHandler<Entities.Tap> connectionHandler)
            : base(connectionHandler)
        {
        }

        public override void Update(Tap entity)
        {
            throw new NotImplementedException();
        }
    }
}

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

    public class KegRepository : EntityRepository<Keg>,  IKegRepository
    {
        public KegRepository(KegCollectionDefinition kegCollectionDefinition)
            : base(kegCollectionDefinition)
        {
        }
    }
}
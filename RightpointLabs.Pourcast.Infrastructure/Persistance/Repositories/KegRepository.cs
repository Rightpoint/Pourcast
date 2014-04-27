namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistance.Collections;

    public class KegRepository : EntityRepository<Keg>,  IKegRepository
    {
        public KegRepository(KegCollectionDefinition kegCollectionDefinition)
            : base(kegCollectionDefinition)
        {
        }
    }
}
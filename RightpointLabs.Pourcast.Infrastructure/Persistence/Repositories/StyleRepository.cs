namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

    public class StyleRepository : EntityRepository<Style>, IStyleRepository
    {
        public StyleRepository(EntityCollectionDefinition<Style> collectionDefinition)
            : base(collectionDefinition)
        {
        }
    }
}
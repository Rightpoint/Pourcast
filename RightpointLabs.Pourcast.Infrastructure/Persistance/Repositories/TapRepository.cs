namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Repositories
{
    using System.Linq;

    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistance.Collections;

    public class TapRepository : EntityRepository<Tap>, ITapRepository
    {
        public TapRepository(TapCollectionDefinition tapCollectionDefinition)
            : base(tapCollectionDefinition)
        {
        }

        public Tap GetByKegId(string kegId)
        {
            return Queryable.Single(t => t.KegId == kegId);
        }
    }
}
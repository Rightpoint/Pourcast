namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System.Linq;

    using Domain.Models;
    using Domain.Repositories;
    using Collections;

    public class TapNotificationStateRepository : EntityRepository<TapNotificationState>, ITapNotificationStateRepository
    {
        public TapNotificationStateRepository(TapNotificationStateCollectionDefinition collectionDefinition)
            : base(collectionDefinition)
        {
        }

        public TapNotificationState GetByTapId(string tapId)
        {
            return Queryable.SingleOrDefault(i => i.TapId == tapId);
        }
    }
}
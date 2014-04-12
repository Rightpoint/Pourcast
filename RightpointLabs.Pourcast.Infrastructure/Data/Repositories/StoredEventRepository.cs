namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class StoredEventRepository<T> : EntityRepository<StoredEvent<T>>, IStoredEventRepository<T> where T : IDomainEvent
    {
        public StoredEventRepository(IMongoConnectionHandler<StoredEvent<T>> connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
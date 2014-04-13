namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class StoredEventRepository : EntityRepository<StoredEvent>, IStoredEventRepository
    {
        static StoredEventRepository()
        {
            BsonClassMap.RegisterClassMap<StoredEvent>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapCreator(e => new StoredEvent(e.Id, e.OccuredOn, e.DomainEvent));
                });
        }

        public StoredEventRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }
    }
}
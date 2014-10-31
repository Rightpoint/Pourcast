namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Collections
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Events;

    public class StoredEventCollectionDefinition : EntityCollectionDefinition<StoredEvent<IDomainEvent>>
    {
        public StoredEventCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(StoredEvent<IDomainEvent>)))
            {
                BsonClassMap.RegisterClassMap<StoredEvent<IDomainEvent>>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }
    }
}

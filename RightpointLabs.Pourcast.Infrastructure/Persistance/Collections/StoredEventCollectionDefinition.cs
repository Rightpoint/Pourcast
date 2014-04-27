namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Collections
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Events;

    public class StoredEventCollectionDefinition : EntityCollectionDefinition<StoredEvent>
    {
        public StoredEventCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(StoredEvent)))
            {
                BsonClassMap.RegisterClassMap<StoredEvent>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }
    }
}

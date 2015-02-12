using System.Linq;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Collections
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
            typeof (IDomainEvent).Assembly.GetTypes().Where(i => typeof (IDomainEvent).IsAssignableFrom(i)).ToList().ForEach(i => BsonClassMap.LookupClassMap(i));
        }
    }
}

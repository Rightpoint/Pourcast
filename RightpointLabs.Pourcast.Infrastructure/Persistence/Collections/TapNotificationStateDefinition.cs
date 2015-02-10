namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Collections
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;

    public class TapNotificationStateCollectionDefinition : EntityCollectionDefinition<TapNotificationState>
    {
        public TapNotificationStateCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TapNotificationState)))
            {
                BsonClassMap.RegisterClassMap<TapNotificationState>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }
    }
}

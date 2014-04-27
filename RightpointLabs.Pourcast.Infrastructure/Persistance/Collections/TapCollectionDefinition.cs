namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Collections
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;

    public class TapCollectionDefinition : EntityCollectionDefinition<Tap>
    {
        public TapCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Tap)))
            {
                BsonClassMap.RegisterClassMap<Tap>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }
    }
}

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Collections
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;

    public class StyleCollectionDefinition : EntityCollectionDefinition<Style>
    {
        public StyleCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Style)))
            {
                BsonClassMap.RegisterClassMap<Style>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }
    }
}
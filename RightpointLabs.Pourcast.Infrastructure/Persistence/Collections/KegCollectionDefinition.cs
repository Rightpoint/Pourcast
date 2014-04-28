namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Collections
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;

    public class KegCollectionDefinition : EntityCollectionDefinition<Keg>
    {
        public KegCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Keg)))
            {
                BsonClassMap.RegisterClassMap<Keg>(
                cm =>
                {
                    cm.AutoMap();
                });
            }
        }
    }
}

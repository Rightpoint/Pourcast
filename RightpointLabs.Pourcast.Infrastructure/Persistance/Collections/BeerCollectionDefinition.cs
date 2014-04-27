namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Collections
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;

    public class BeerCollectionDefinition : EntityCollectionDefinition<Beer>
    {
        public BeerCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Beer)))
            {
                BsonClassMap.RegisterClassMap<Beer>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }
    }
}

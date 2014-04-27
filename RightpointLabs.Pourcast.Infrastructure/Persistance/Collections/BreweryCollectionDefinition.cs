namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Collections
{
    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;

    public class BreweryCollectionDefinition : EntityCollectionDefinition<Brewery>
    {
        public BreweryCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Brewery)))
            {
                BsonClassMap.RegisterClassMap<Brewery>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }
    }
}

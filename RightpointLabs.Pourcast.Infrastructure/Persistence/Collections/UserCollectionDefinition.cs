namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Collections
{
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver.Builders;

    using RightpointLabs.Pourcast.Domain.Models;

    public class UserCollectionDefinition : EntityCollectionDefinition<User>
    {
        public UserCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(
                    cm =>
                    {
                        cm.AutoMap();
                        cm.MapField("_roleIds").SetElementName("RoleIds");
                    });
            }

            Collection.EnsureIndex(new IndexKeysBuilder().Ascending("Username"));
        }
    }
}

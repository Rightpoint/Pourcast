namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Collections
{
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver.Builders;

    using RightpointLabs.Pourcast.Domain.Models;

    public class RoleCollectionDefinition : EntityCollectionDefinition<Role>
    {
        public RoleCollectionDefinition(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Role)))
            {
                BsonClassMap.RegisterClassMap<Role>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapField("_userIds").SetElementName("UserIds");
                });
            }

            Collection.EnsureIndex(new IndexKeysBuilder().Ascending("Name"));
        }
    }
}

namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Collections
{
    using MongoDB.Bson.Serialization;

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
        }
    }
}

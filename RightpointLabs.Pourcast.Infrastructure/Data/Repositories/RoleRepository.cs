namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;

    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class RoleRepository : EntityRepository<Role>, IRoleRepository
    {
        static RoleRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Role)))
            {
                BsonClassMap.RegisterClassMap<Role>(
                cm =>
                {
                    cm.AutoMap();
                });
            }
        }

        public RoleRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public Role GetByName(string name)
        {
            return Queryable.SingleOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

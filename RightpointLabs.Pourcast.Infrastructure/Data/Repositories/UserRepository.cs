namespace RightpointLabs.Pourcast.Infrastructure.Data.Repositories
{
    using System;
    using System.Linq;

    using MongoDB.Bson.Serialization;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        static UserRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>(
                    cm =>
                    {
                        cm.AutoMap();
                    });
            }
        }

        public UserRepository(IMongoConnectionHandler connectionHandler)
            : base(connectionHandler)
        {
        }

        public User GetByUsername(string username)
        {
            return Queryable.SingleOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

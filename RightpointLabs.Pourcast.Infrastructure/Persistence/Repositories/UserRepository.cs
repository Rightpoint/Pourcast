using System;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistence.Collections;

    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        public UserRepository(UserCollectionDefinition userCollectionDefinition)
            : base(userCollectionDefinition)
        {
        }

        public User GetByUsername(string username)
        {
            // TODO: use a Regex so we can drop the ToList() and push the work to Mongo
            return Queryable.ToList().SingleOrDefault(x => string.Equals(x.Username, username, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<User> GetUsersInRole(string id)
        {
            return Queryable.Where(x => x.RoleIds.Contains(id));
        }
    }
}

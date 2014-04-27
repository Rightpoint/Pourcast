namespace RightpointLabs.Pourcast.Infrastructure.Persistance.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Infrastructure.Persistance.Collections;

    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        public UserRepository(UserCollectionDefinition userCollectionDefinition)
            : base(userCollectionDefinition)
        {
        }

        public User GetByUsername(string username)
        {
            return Queryable.SingleOrDefault(x => x.Username == username);
        }

        public IEnumerable<User> GetUsersInRole(string id)
        {
            return Queryable.Where(x => x.RoleIds.Contains(id));
        }
    }
}

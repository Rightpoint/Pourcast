using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using System.Linq;

    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class UserRepository : TableRepository<User>, IUserRepository
    {
        public UserRepository(CloudTableClient client) : base(client)
        {
        }

        public User GetByUsername(string username)
        {
            return this.GetAll().SingleOrDefault(i => i.Username == username);
        }
    }
}

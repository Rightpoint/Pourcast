namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IUserRepository : IRepository
    {
        User GetById(string id);

        IEnumerable<User> GetAll();

        User GetByUsername(string username);

        void Insert(User user);

        void Update(User user);
    }
}

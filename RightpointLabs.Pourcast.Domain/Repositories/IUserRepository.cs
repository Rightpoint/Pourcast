namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IUserRepository
    {
        User GetById(string id);

        IEnumerable<User> GetAll();

        User GetByUsername(string username);

        IEnumerable<User> GetUsersInRole(string id);

        void Add(User user);

        void Update(User user);
    }
}

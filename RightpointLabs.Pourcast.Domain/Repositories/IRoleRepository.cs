namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IRoleRepository
    {
        Role GetById(string id);

        IEnumerable<Role> GetAll();

        Role GetByName(string name);

        string NextIdentity();

        void Add(Role role);

        void Update(Role role);
    }
}

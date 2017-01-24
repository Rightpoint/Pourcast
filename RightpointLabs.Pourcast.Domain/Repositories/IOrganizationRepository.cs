namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IOrganizationRepository : IRepository
    {
        Organization GetById(string id);

        IEnumerable<Organization> GetAll();

        void Insert(Organization organization);

        void Update(Organization organization);
    }
}

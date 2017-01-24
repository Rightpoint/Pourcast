using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class OrganizationRepository : TableRepository<Organization>,  IOrganizationRepository
    {
        public OrganizationRepository(CloudTableClient client) : base(client)
        {
        }
    }
}
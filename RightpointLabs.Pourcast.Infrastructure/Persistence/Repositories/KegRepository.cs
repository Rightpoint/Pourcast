using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class KegRepository : TableByOrganizationRepository<Keg>,  IKegRepository
    {
        public KegRepository(CloudTableClient client) : base(client)
        {
        }
    }
}
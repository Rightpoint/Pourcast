using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class KegListRepository : TableByOrganizationRepository<KegList>,  IKegListRepository
    {
        public KegListRepository(CloudTableClient client) : base(client)
        {
        }
    }
}
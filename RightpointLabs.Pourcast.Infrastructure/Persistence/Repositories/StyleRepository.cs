using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class StyleRepository : TableRepository<Style>, IStyleRepository
    {
        public StyleRepository(CloudTableClient client) : base(client)
        {
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class LocationRepository : TableByOrganizationRepository<Location>,  ILocationRepository
    {
        public LocationRepository(CloudTableClient client) : base(client)
        {
        }

        public IEnumerable<Location> GetAllByParentLocation(string organizationId, string parentLocationId)
        {
            return GetAll(organizationId).Where(i => i.ParentLocationId == parentLocationId).ToList();
        }
    }
}
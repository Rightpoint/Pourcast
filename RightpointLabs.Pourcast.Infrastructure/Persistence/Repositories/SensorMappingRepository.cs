using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class SensorMappingRepository : TableByOrganizationRepository<SensorMapping>, ISensorMappingRepository
    {
        public SensorMappingRepository(CloudTableClient client) : base(client)
        {
        }

        public IEnumerable<SensorMapping> GetAllForSensor(string sensorId, string organizationId)
        {
            return this.GetAll(organizationId).Where(i => i.SensorId == sensorId).ToList();
        }

        public IEnumerable<SensorMapping> GetAllForTap(string tapId, string organizationId)
        {
            return this.GetAll(organizationId).Where(i => i.TapId == tapId).ToList();
        }

        public IEnumerable<SensorMapping> GetAllForLocation(string locationId, string organizationId)
        {
            return this.GetAll(organizationId).Where(i => i.LocationId == locationId).ToList();
        }
    }
}
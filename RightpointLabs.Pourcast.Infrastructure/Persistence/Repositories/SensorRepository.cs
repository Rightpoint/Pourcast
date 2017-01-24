using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace RightpointLabs.Pourcast.Infrastructure.Persistence.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class SensorRepository : TableByOrganizationRepository<Sensor>,  ISensorRepository
    {
        public SensorRepository(CloudTableClient client) : base(client)
        {
        }

        public IEnumerable<Sensor> GetAllForDevice(string deviceId, string organizationId)
        {
            return this.GetAll(organizationId).Where(i => i.DeviceId == deviceId).ToList();
        }
    }
}
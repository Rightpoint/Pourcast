using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    public interface ISensorMappingRepository : IRepository
    {
        SensorMapping GetById(string id, string organizationId);

        IEnumerable<SensorMapping> GetAllForSensor(string sensorId, string organizationId);

        IEnumerable<SensorMapping> GetAllForTap(string tapId, string organizationId);

        IEnumerable<SensorMapping> GetAllForLocation(string locationId, string organizationId);

        void Insert(SensorMapping sensorMapping);

        void Update(SensorMapping sensorMapping);
    }
}

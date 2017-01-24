using RightpointLabs.Pourcast.Domain.Models;

namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System;
    using System.Collections.Generic;

    public interface ISensorRepository : IRepository
    {
        Sensor GetById(string id, string organizationId);

        IEnumerable<Sensor> GetAllForDevice(string deviceId, string organizationId);

        void Insert(Sensor sensor);

        void Update(Sensor sensor);
    }
}

namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IDeviceRepository : IRepository
    {
        Device GetById(string id);
        Device GetById(string organizationId, string id);
        IEnumerable<Device> GetAll(string organizationId);
        void Update(Device device);
        void Insert(Device device);
    }
}

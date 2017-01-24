namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface ILocationRepository : IRepository
    {
        Location GetById(string id, string organizationId);

        IEnumerable<Location> GetAll(string organizationId);

        IEnumerable<Location> GetAllByParentLocation(string organizationId, string parentLocationId);

        void Insert(Location location);

        void Update(Location location);
    }
}

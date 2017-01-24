namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface ITapRepository : IRepository
    {
        Tap GetById(string id, string organizationId);
        IEnumerable<Tap> GetAll(string organizationId);
        Tap GetByName(string name, string organizationId);
        void Insert(Tap tap);
        void Update(Tap tap);
    }
}
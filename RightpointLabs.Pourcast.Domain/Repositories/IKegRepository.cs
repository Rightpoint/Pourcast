namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IKegRepository : IRepository
    {
        Keg GetById(string id);

        Keg GetById(string id, string organizationId);

        IEnumerable<Keg> GetAll(string organizationId);

        void Update(Keg keg);
        
        void Insert(Keg keg);
    }
}
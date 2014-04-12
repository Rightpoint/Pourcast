namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IKegRepository
    {
        Keg GetById(string id);
        IEnumerable<Keg> GetAll();
        void Update(Keg keg);
        string NextIdentity();
    }
}
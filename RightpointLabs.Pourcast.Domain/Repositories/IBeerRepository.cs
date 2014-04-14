namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBeerRepository
    {
        Beer GetById(string id);
        
        IEnumerable<Beer> GetAll();
        
        string NextIdentity();

        void Add(Beer beer);
    }
}
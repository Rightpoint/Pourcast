namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBreweryRepository
    {
        Brewery GetById(string id);
        
        IEnumerable<Brewery> GetAll();

        void Add(Brewery brewery);

        void Update(Brewery brewery);

        string NextIdentity();
    }
}
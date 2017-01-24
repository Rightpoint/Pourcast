namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBreweryRepository : IRepository
    {
        Brewery GetById(string id);

        Brewery GetByName(string name);

        IEnumerable<Brewery> GetAll();

        void Insert(Brewery brewery);

        void Update(Brewery brewery);
    }
}
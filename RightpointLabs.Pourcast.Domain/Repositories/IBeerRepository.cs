namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBeerRepository : IRepository
    {
        Beer GetById(string id);
        IEnumerable<Beer> GetAll();
        IEnumerable<Beer> GetAllByName(string name);
        IEnumerable<Beer> GetByBreweryId(string breweryId);
        void Update(Beer beer);
        void Insert(Beer beer);
    }
}

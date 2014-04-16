namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBeerRepository
    {
        Beer GetById(string id);
        
        IEnumerable<Beer> GetAll();

        IEnumerable<Beer> GetAllByName(string name);
<<<<<<< HEAD

        IEnumerable<Beer> GetByBreweryId(string breweryId); 

=======
        
>>>>>>> Adding create beer files
        string NextIdentity();

        void Add(Beer beer);
    }
}
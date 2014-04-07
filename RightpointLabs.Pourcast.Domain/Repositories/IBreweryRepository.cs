namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBreweryRepository
    {
        Brewery GetById(string id);
        IEnumerable<Brewery> GetAll();
        string NextIdentity();
    }
}
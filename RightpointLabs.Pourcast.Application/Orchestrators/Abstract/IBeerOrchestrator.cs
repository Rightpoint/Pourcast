namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBeerOrchestrator
    {
        IEnumerable<Beer> GetBeers();

        IEnumerable<Beer> GetBeersByName(string name);

        IEnumerable<Beer> GetBeersByBrewery(int breweryId);

        string CreateBeer(string name);
    }
}
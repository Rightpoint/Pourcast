namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Payloads;
    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBeerOrchestrator
    {
        IEnumerable<Beer> GetBeers();

        IEnumerable<BeerOnTap> GetBeersOnTap(string organizationId);

        BeerOnTap GetBeerOnTap(string tapId, string organizationId);
            
        IEnumerable<Beer> GetByName(string name);

        IEnumerable<Beer> GetByBrewery(string breweryId);

        Beer GetById(string id);

        void Save(Beer beer);

        string CreateBeer(string name, double abv, double baScore, string styleId, string breweryId);
    }
}
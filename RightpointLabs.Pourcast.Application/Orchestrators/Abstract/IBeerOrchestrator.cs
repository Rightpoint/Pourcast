using RightpointLabs.Pourcast.Application.Commands;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Payloads;
    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBeerOrchestrator
    {
        IEnumerable<Beer> GetBeers();

        IEnumerable<BeerOnTap> GetBeersOnTap();

        BeerOnTap GetBeerOnTap(string tapId);
            
        IEnumerable<Beer> GetByName(string name);

        IEnumerable<Beer> GetByBrewery(string breweryId);

        Beer GetById(string id);

        void Save(Beer beer);

        string CreateBeer(string name, double abv, int baScore, string style, string color, string glass, string breweryId);
    }
}
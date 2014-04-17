using RightpointLabs.Pourcast.Application.Commands;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBreweryOrchestrator
    {
        IEnumerable<Brewery> GetBreweries();
        Brewery GetById(string id);
<<<<<<< HEAD
        string Create(CreateBrewery breweryCommand);
        EditBrewery EditBrewery(string breweryId);
        void EditBrewery(EditBrewery editBreweryCommand);
=======
        Brewery GetShell();
        void Create(Brewery brewery);
>>>>>>> Can create brewery.
    }
}
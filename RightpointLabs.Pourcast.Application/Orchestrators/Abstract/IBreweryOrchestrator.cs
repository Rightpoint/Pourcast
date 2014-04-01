namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBreweryOrchestrator
    {
        IEnumerable<Brewery> GetBreweries();
    }
}
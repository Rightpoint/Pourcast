using System.Collections.Generic;
using RighpointLabs.Pourcast.Orchestrator.Models;

namespace RighpointLabs.Pourcast.Orchestrator.Abstract
{
    public interface IBreweryOrchestrator
    {
        List<Brewery> GetBreweries();
    }
}
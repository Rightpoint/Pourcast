using System.Collections.Generic;
using RighpointLabs.Pourcast.Orchestrator.Models;

namespace RighpointLabs.Pourcast.Orchestrator.Abstract
{
    public interface IBeerOrchestrator
    {
        IEnumerable<Models.Beer> GetAll();
        IEnumerable<Models.Beer> GetAllByBrewer(Brewery brewery);

    }
}
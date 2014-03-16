using System.Collections.Generic;
using RighpointLabs.Pourcast.Orchestrator.Models;

namespace RighpointLabs.Pourcast.Orchestrator.Abstract
{
    public interface IKegOrchestrator
    {
        IEnumerable<Keg> GetAll();
        IEnumerable<Keg> GetOnTap();
    }
}
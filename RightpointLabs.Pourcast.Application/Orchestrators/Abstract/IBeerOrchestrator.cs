namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IBeerOrchestrator
    {
        IEnumerable<Beer> GetAll();
        IEnumerable<Beer> GetAllByBrewer(int breweryId);

    }
}
namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IKegOrchestrator
    {
        IEnumerable<Keg> GetAll();
        IEnumerable<Keg> GetOnTap();
    }
}
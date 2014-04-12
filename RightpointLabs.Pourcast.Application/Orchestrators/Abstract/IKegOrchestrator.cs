namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IKegOrchestrator
    {
        IEnumerable<Keg> GetKegs();

        Keg GetKeg(string kegId);

        IEnumerable<Keg> GetKegsOnTap();

        Keg GetKegOnTap(string tapId);
    }
}
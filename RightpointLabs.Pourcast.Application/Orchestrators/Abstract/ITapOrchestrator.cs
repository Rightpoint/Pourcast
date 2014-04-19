namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface ITapOrchestrator
    {
        Tap GetTapById(string id);

        IEnumerable<Tap> GetTaps();

        void StartPourFromTap(string tapId);

        void StopPourFromTap(string tapId, double volume);

        void RemoveKegFromTap(string tapId);

        void TapKeg(string tapId, string kegId);

        string CreateTap(TapName name);
    }
}

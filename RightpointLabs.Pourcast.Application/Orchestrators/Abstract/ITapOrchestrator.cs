namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface ITapOrchestrator
    {
        Tap GetTapById(string id, string organizationId);

        IEnumerable<Tap> GetTaps(string organizationId);

        //void StartPourFromTap(string tapId);

        //void PouringFromTap(string id, double volume);

        //void StopPourFromTap(string tapId, double volume);

        //void RemoveKegFromTap(string tapId);

        //void TapKeg(string tapId, string kegId);

        string CreateTap(string name, string organizationId);

        //string CreateTap(string name, string kegId);

        Tap GetByName(string name, string organizationId);

        void Save(Tap tap);

        //void UpdateTemperature(string id, double temperatureF);
    }
}

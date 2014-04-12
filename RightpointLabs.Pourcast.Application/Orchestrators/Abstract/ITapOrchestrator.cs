namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using RightpointLabs.Pourcast.Domain.Models;

    public interface ITapOrchestrator
    {
        void PourBeerFromTap(string tapId, double volume);

        void SwitchKegOnTap(string tapId, string kegId);

        void CreateTap(TapName name);
    }
}

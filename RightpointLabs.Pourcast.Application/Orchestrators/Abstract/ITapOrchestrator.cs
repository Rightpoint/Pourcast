namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using RightpointLabs.Pourcast.Domain.Models;

    public interface ITapOrchestrator
    {
        void PourBeerFromTap(string tapId, double volume);

        void RemoveKegFromTap(string tapId);

        void CreateTap(TapName name);
    }
}

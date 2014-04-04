namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    public interface ITapOrchestrator
    {
        void PourBeerFromTap(int tapId, double volume);
    }
}

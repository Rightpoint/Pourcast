namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    public interface ITapOrchestrator
    {
        void PourBeerFromTap(string tapId, double volume);
    }
}

using RightpointLabs.Pourcast.Application.Transactions;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    public interface IStatusOrchestrator
    {
        [Transactional]
        void Heartbeat();

        [Transactional]
        void LogMessage(string message);
    }
}
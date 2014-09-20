using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Application.Transactions;

    public class StatusOrchestrator : IStatusOrchestrator
    {
        [Transactional]
        public void Heartbeat()
        {
            DomainEvents.Raise(new Heartbeat());
        }

        [Transactional]
        public void LogMessage(string message)
        {
            DomainEvents.Raise(new LogMessage(message));
        }
    }
}
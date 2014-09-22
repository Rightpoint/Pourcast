namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Events;

    public class HeartbeatClientHandler : IEventHandler<Heartbeat>
    {
        private readonly IConnectionManager _connectionManager;

        public HeartbeatClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Handle(Heartbeat domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            TransactionExtensions.WaitForTransactionCompleted(() => context.Clients.All.Heartbeat(domainEvent));
        }
    }
}
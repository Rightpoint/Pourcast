namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Domain.Events;

    public class KegTappedClientHandler : TransactionDependentEventHandler<KegTapped>
    {
        private readonly IConnectionManager _connectionManager;

        public KegTappedClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        protected override void HandleAfterTransaction(KegTapped domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.KegTapped(domainEvent);
        }
    }
}
namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Domain.Events;

    public class BeerPourEndedClientHandler : TransactionDependentEventHandler<BeerPourStopped>
    {
        private readonly IConnectionManager _connectionManager;

        public BeerPourEndedClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        protected override void HandleAfterTransaction(BeerPourStopped domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.BeerPourEnded(domainEvent);
        }
    }
}
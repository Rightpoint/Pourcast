namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Domain.Events;

    public class BeerPourStartedClientHandler : TransactionDependentEventHandler<BeerPourStarted>
    {
        private readonly IConnectionManager _connectionManager;

        public BeerPourStartedClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        protected override void HandleAfterTransaction(BeerPourStarted domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.BeerPourStarted(domainEvent);
        }
    }
}
namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Domain.Events;

    public class BeerPouredClientHandler : TransactionDependentEventHandler<BeerPoured>
    {
        private readonly IConnectionManager _connectionManager;

        public BeerPouredClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        protected override void HandleAfterTransaction(BeerPoured domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.BeerPoured(domainEvent);
        }
    }
}
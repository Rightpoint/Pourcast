namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Domain.Events;

    public class PourStoppedClientHandler : IEventHandler<PourStopped>
    {
        private readonly IConnectionManager _connectionManager;

        public PourStoppedClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Handle(PourStopped domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.BeerPourStopped(domainEvent);
        }
    }
}
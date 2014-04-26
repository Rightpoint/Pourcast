namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Domain.Events;

    public class KegTappedClientHandler : IEventHandler<KegTapped>
    {
        private readonly IConnectionManager _connectionManager;

        public KegTappedClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Handle(KegTapped domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.KegTapped(domainEvent);
        }
    }
}
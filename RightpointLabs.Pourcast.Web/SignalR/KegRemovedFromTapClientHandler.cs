namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Domain.Events;

    public class KegRemovedFromTapClientHandler : TransactionDependentEventHandler<KegRemovedFromTap>
    {
        private readonly IConnectionManager _connectionManager;

        public KegRemovedFromTapClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        protected override void HandleAfterTransaction(KegRemovedFromTap domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            context.Clients.All.KegRemovedFromTap(domainEvent);
        }
    }
}
namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Events;

    public class KegRemovedFromTapClientHandler : IEventHandler<KegRemovedFromTap>
    {
        private readonly IConnectionManager _connectionManager;

        public KegRemovedFromTapClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Handle(KegRemovedFromTap domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            TransactionExtensions.WaitForTransactionCompleted(() => context.Clients.All.KegRemovedFromTap(domainEvent));
        }
    }
}
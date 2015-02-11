namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR.Infrastructure;

    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Events;

    public class PictureRequestedClientHandler : IEventHandler<PictureRequested>
    {
        private readonly IConnectionManager _connectionManager;

        public PictureRequestedClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Handle(PictureRequested domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            TransactionExtensions.WaitForTransactionCompleted(() => context.Clients.All.PictureRequested(domainEvent));
        }
    }
}
using Microsoft.AspNet.SignalR.Infrastructure;
using RightpointLabs.Pourcast.Application.Transactions;
using RightpointLabs.Pourcast.Domain.Events;

namespace RightpointLabs.Pourcast.Web.SignalR
{
    public class PouringClientHandler : IEventHandler<Pouring>
    {
        private readonly IConnectionManager _connectionManager;

        public PouringClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Handle(Pouring domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            TransactionExtensions.WaitForTransactionCompleted(() => context.Clients.All.Pouring(domainEvent));
        }
    }
}
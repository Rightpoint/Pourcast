using Microsoft.AspNet.SignalR.Infrastructure;
using RightpointLabs.Pourcast.Application.Transactions;
using RightpointLabs.Pourcast.Domain.Events;

namespace RightpointLabs.Pourcast.Web.SignalR
{
    public class KegTemperatureChangedClientHandler : IEventHandler<KegTemperatureChanged>
    {
        private readonly IConnectionManager _connectionManager;

        public KegTemperatureChangedClientHandler(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void Handle(KegTemperatureChanged domainEvent)
        {
            var context = _connectionManager.GetHubContext<EventsHub>();

            TransactionExtensions.WaitForTransactionCompleted(() => context.Clients.All.KegTemperatureChanged(domainEvent));
        }
    }
}
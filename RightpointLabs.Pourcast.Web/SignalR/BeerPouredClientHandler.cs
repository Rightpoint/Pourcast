namespace RightpointLabs.Pourcast.Web.SignalR
{
    using Microsoft.AspNet.SignalR;

    using RightpointLabs.Pourcast.Application.EventHandlers;
    using RightpointLabs.Pourcast.Domain.Events;

    public class BeerPouredClientHandler : TransactionDependentEventHandler<BeerPoured>
    {
        protected override void HandleAfterTransaction(BeerPoured domainEvent)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();

            context.Clients.All.beerPoured(domainEvent);
        }
    }
}
namespace RightpointLabs.Pourcast.Infrastructure.EventHandlers
{
    using RightpointLabs.Pourcast.Domain.Events;

    public class KegNearingEmptyEventHandler : IDomainEventHandler<KegNearingEmpty>
    {
        public void HandleEvent(KegNearingEmpty domainEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}

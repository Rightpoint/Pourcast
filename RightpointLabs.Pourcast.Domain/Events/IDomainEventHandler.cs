namespace RightpointLabs.Pourcast.Domain.Events
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        void HandleEvent(T domainEvent);
    }
}

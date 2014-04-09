namespace RightpointLabs.Pourcast.Domain.Events
{
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        void HandleEvent(T domainEvent);
    }
}

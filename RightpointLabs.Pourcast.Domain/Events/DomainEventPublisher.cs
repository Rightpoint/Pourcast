namespace RightpointLabs.Pourcast.Domain.Events
{
    using System.Collections;
    using System.Linq;

    public static class DomainEventPublisher
    {
        private static readonly ArrayList _handlers;

        static DomainEventPublisher()
        {
            _handlers = new ArrayList();
        }

        public static void Subscribe<T>(IDomainEventHandler<T> handler) where T : IDomainEvent
        {
            _handlers.Add(handler);
        }

        public static void Publish<T>(T domainEvent) where T : IDomainEvent
        {
            var relevantHandlers = _handlers.OfType<IDomainEventHandler<T>>();

            foreach (var handler in relevantHandlers)
            {
                handler.HandleEvent(domainEvent);
            }
        }
    }
}

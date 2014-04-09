namespace RightpointLabs.Pourcast.Domain.Events
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DomainEventPublisher
    {
        private static readonly List<IDomainEventHandler<IDomainEvent>> _handlers;

        static DomainEventPublisher()
        {
            _handlers = new List<IDomainEventHandler<IDomainEvent>>();
        }

        public static void Subscribe(IDomainEventHandler<IDomainEvent> handler)
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

using System;

namespace RightpointLabs.Pourcast.Infrastructure.EventHandlers
{
    using RightpointLabs.Pourcast.Domain.Events;

    public class KegEmptiedEventHandler : IDomainEventHandler<KegEmptied>
    {
        public void HandleEvent(KegEmptied domainEvent)
        {
            throw new NotImplementedException();
        }
    }
}

namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Events;

    public interface IStoredEventRepository
    {
        void Add(StoredEvent domainEvent);
        
        IEnumerable<StoredEvent> GetAll();
        
        StoredEvent GetById(string id);
        
        string NextIdentity();
    }
}

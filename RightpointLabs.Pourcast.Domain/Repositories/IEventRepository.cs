namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Events;

    public interface IStoredEventRepository<T> where T : IDomainEvent
    {
        void Add(StoredEvent<T> domainEvent);
        
        IEnumerable<StoredEvent<T>> GetAll();
        
        StoredEvent<T> GetById(string id);
        
        string NextIdentity();
    }
}

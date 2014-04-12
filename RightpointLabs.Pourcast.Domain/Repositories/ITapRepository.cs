namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface ITapRepository
    {
        void Add(Tap tap);
        
        IEnumerable<Tap> GetAll();
        
        Tap GetById(string id);
        
        string NextIdentity();

        void Update(Tap tap);
    }
}
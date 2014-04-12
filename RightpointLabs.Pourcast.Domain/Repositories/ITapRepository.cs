namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface ITapRepository
    {
        IEnumerable<Tap> GetAll();
        Tap GetById(string id);
    }
}
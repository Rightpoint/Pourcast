using System.Collections.Generic;

namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;

    public interface IStyleRepository
    {
        Style GetById(string id);
        IEnumerable<Style> GetAll();
        string NextIdentity();
        void Update(Style style);
        void Add(Style style);
    }
}

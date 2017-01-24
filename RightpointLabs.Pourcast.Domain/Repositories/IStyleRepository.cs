using System.Collections.Generic;

namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using RightpointLabs.Pourcast.Domain.Models;

    public interface IStyleRepository : IRepository
    {
        Style GetById(string id);
        IEnumerable<Style> GetAll();
        void Update(Style style);
        void Insert(Style style);
    }
}

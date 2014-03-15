using System.Collections.Generic;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Repository.Abstract
{
    public interface IBreweryRepository : IEntityRepository<Brewery>
    {
        IEnumerable<Brewery> GetAll();
    }
}
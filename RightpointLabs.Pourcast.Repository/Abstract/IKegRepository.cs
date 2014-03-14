using System.Collections.Generic;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Repository.Abstract
{
    public interface IKegRepository : IEntityRepository<Keg>
    {
        List<Keg> GetAll();
    }
}
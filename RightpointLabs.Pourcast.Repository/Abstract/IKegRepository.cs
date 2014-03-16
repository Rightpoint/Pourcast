using System.Collections.Generic;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Repository.Abstract
{
    public interface IKegRepository : IEntityRepository<Keg>
    {
        IEnumerable<Keg> GetAll();
        /// <summary>
        /// Gets all kegs currently on tap
        /// </summary>
        /// <returns></returns>
        IEnumerable<Keg> OnTap();
    }
}
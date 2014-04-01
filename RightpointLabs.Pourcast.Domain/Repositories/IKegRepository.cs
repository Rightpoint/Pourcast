namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IKegRepository
    {
        Keg GetById(string id);
        IEnumerable<Keg> GetAll();
        /// <summary>
        /// Gets all kegs currently on tap
        /// </summary>
        /// <returns></returns>
        IEnumerable<Keg> OnTap();
    }
}
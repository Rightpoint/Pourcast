using System.Collections.Generic;
using RightpointLabs.Pourcast.DataModel.Entities;

namespace RightpointLabs.Pourcast.Repository.Abstract
{
    public interface IKegRepository
    {
        List<Keg> GetAll();
    }
}
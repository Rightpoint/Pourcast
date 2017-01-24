namespace RightpointLabs.Pourcast.Domain.Repositories
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IKegListRepository : IRepository
    {
        KegList GetById(string id, string organizationId);

        void Update(KegList kegList);
        
        void Insert(KegList kegList);
    }
}
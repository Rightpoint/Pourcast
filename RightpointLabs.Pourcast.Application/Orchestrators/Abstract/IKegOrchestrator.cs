using RightpointLabs.Pourcast.Application.Transactions;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Domain.Models;

    public interface IKegOrchestrator
    {
        //IEnumerable<Keg> GetKegs();

        //IEnumerable<Keg> GetKegs(bool isEmpty);
            
        Keg GetKeg(string kegId, string organizationId);
        
        //IEnumerable<Keg> GetKegsOnTap();
            
        //Keg GetKegOnTap(string tapId);

        string CreateKeg(string beerId, string kegTypeId, string organizationId);
        
        //void UpdateCapacityAndPoured(string kegId, double capacity, double amountOfBeerPoured);
    }
}
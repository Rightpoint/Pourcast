using RightpointLabs.Pourcast.Application.Transactions;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Abstract
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Payloads;
    using RightpointLabs.Pourcast.Domain.Models;

    public interface IKegOrchestrator
    {
        IEnumerable<Keg> GetKegs();

        IEnumerable<Keg> GetKegs(bool isEmpty);
            
        Keg GetKeg(string kegId);
        
        IEnumerable<Keg> GetKegsOnTap();
            
        Keg GetKegOnTap(string tapId);

        string CreateKeg(string beerId, double capacity);
        
        void UpdateCapacityAndPoured(string kegId, double capacity, double amountOfBeerPoured);

        IEnumerable<KegBurndownPoint> GetKegBurndown(string id);
    }
}
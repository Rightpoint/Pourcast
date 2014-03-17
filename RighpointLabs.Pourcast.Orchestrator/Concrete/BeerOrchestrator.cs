using System.Collections.Generic;
using RighpointLabs.Pourcast.Orchestrator.Abstract;
using RighpointLabs.Pourcast.Orchestrator.Models;
using RightpointLabs.Pourcast.Repository.Abstract;

namespace RighpointLabs.Pourcast.Orchestrator.Concrete
{
    public class BeerOrchestrator : BaseOrchestrator, IBeerOrchestrator
    {
        private readonly IBeerRepository _beerRepository;

        public IEnumerable<Beer> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Beer> GetAllByBrewer(Brewery brewery)
        {
            throw new System.NotImplementedException();
        }
    }
}
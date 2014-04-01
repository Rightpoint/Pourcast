namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

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
namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerOrchestrator : BaseOrchestrator, IBeerOrchestrator
    {
        private readonly IBeerRepository _beerRepository;

        public BeerOrchestrator(IBeerRepository beerRepository)
        {
            if (beerRepository == null) throw new ArgumentNullException("beerRepository");

            _beerRepository = beerRepository;
        }

        public IEnumerable<Beer> GetAll()
        {
            return _beerRepository.GetAll();
        }

        public IEnumerable<Beer> GetAllByBrewer(int breweryId)
        {
            throw new System.NotImplementedException();
        }
    }
}
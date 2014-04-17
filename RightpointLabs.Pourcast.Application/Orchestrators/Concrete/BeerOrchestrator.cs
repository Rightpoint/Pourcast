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

        public IEnumerable<Beer> GetBeers()
        {
            return _beerRepository.GetAll();
        }

        public IEnumerable<Beer> GetBeersByBrewery(string breweryId)
        {
            return _beerRepository.GetByBreweryId(breweryId);
        }

        public string CreateBeer(string name)
        {
            var id = _beerRepository.NextIdentity();
            var beer = new Beer(id, name);

            _beerRepository.Add(beer);

            return id;
        }


        public IEnumerable<Beer> GetBeersByName(string name)
        {
            return _beerRepository.GetAllByName(name);
        }
    }
}
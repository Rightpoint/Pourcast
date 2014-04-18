using System.Diagnostics;
using RightpointLabs.Pourcast.Application.Commands;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Transactions;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerOrchestrator : BaseOrchestrator, IBeerOrchestrator
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IBreweryOrchestrator _breweryOrchestrator;

        public BeerOrchestrator(IBeerRepository beerRepository, IBreweryOrchestrator breweryOrchestrator)
        {
            if (beerRepository == null) throw new ArgumentNullException("beerRepository");
            if(breweryOrchestrator == null) throw new ArgumentNullException("breweryOrchestrator");

            _beerRepository = beerRepository;
            _breweryOrchestrator = breweryOrchestrator;
        }

        public IEnumerable<Beer> GetBeers()
        {
            return _beerRepository.GetAll();
        }

        public IEnumerable<Beer> GetByName(string name)
        {
            return _beerRepository.GetAllByName(name);
        }

        public IEnumerable<Beer> GetBeersByBrewery(string breweryId)
        {
            return _beerRepository.GetByBreweryId(breweryId);
        }

        public Beer GetById(string id)
        {
            return _beerRepository.GetById(id);
        }

        public string CreateBeer(CreateBeer createBeerCommand)
        {
            var id = string.Empty;

            using (var scope = new TransactionScope())
            {
                id = _beerRepository.NextIdentity();
                var beer = new Beer(id, createBeerCommand.Name)
                {
                    ABV = createBeerCommand.ABV,
                    BAScore = createBeerCommand.BAScore,
                    BreweryId = createBeerCommand.BreweryId,
                    Color = createBeerCommand.Color,
                    Glass = createBeerCommand.Glass,
                    Style = createBeerCommand.Style,
                    RPScore = 0
                };

                _beerRepository.Add(beer);

                scope.Complete();
            }

            return id;
        }

        public CreateBeer CreateBeer(string breweryId)
        {
            var brewery = _breweryOrchestrator.GetById(breweryId);
            if (brewery == null)
                return null;
            return new CreateBeer()
            {
                BreweryId = brewery.Id,
                BreweryName = brewery.Name
            };
        }

        public IEnumerable<Beer> GetBeersByName(string name)
        {
            return _beerRepository.GetAllByName(name);
        }
    }
}
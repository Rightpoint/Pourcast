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

<<<<<<< HEAD
<<<<<<< HEAD
        public IEnumerable<Beer> GetBeersByBrewery(string breweryId)
=======
        public IEnumerable<Beer> GetByName(string name)
        {
            return _beerRepository.GetAllByName(name);
        }

        public IEnumerable<Beer> GetBeersByBrewery(int breweryId)
>>>>>>> Adding create beer files
        {
=======
        public IEnumerable<Beer> GetBeersByBrewery(string breweryId)
        {
>>>>>>> Can create brewery.
            return _beerRepository.GetByBreweryId(breweryId);
        }

        public string CreateBeer(CreateBeer createBeerCommand)
        {
            var id = _beerRepository.NextIdentity();
            using (var scope = new TransactionScope())
            {
<<<<<<< HEAD
                id = _beerRepository.NextIdentity();
=======
>>>>>>> Can create brewery and can add/create beers for the brewery.  Also, added commands
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

<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> Can create brewery and can add/create beers for the brewery.  Also, added commands
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
<<<<<<< HEAD
=======
>>>>>>> Can create brewery.
=======
>>>>>>> Can create brewery and can add/create beers for the brewery.  Also, added commands

        public IEnumerable<Beer> GetBeersByName(string name)
        {
            return _beerRepository.GetAllByName(name);
        }
    }
}
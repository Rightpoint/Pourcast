using RightpointLabs.Pourcast.Application.Commands;

namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Transactions;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Payloads;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class BeerOrchestrator : BaseOrchestrator, IBeerOrchestrator
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IBreweryRepository _breweryRepository;

        private readonly ITapRepository _tapRepository;

        private readonly IKegRepository _kegRepository;

        public BeerOrchestrator(IBeerRepository beerRepository, IBreweryRepository breweryRepository, ITapRepository tapRepository, IKegRepository kegRepository)
        {
            if (beerRepository == null) throw new ArgumentNullException("beerRepository");
            if(breweryRepository == null) throw new ArgumentNullException("breweryRepository");
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");

            _beerRepository = beerRepository;
            _breweryRepository = breweryRepository;
            _tapRepository = tapRepository;
            _kegRepository = kegRepository;
        }

        public IEnumerable<Beer> GetBeers()
        {
            return _beerRepository.GetAll();
        }

        public IEnumerable<BeerOnTap> GetBeersOnTap()
        {
            var taps = _tapRepository.GetAll();

            foreach (var tap in taps)
            {
                var keg = _kegRepository.GetById(tap.KegId);
                var beer = _beerRepository.GetById(keg.BeerId);
                var brewery = _breweryRepository.GetById(beer.BreweryId);

                yield return new BeerOnTap()
                {
                    Tap = tap,
                    Keg = keg,
                    Beer = beer,
                    Brewery = brewery
                };
            }
        }

        public BeerOnTap GetBeerOnTap(string tapId)
        {
            var tap = _tapRepository.GetById(tapId);
            var keg = _kegRepository.GetById(tap.KegId);
            var beer = _beerRepository.GetById(keg.BeerId);
            var brewery = _breweryRepository.GetById(beer.BreweryId);

            return new BeerOnTap()
            {
                Tap = tap,
                Keg = keg,
                Beer = beer,
                Brewery = brewery
            };
        }

        public IEnumerable<Beer> GetByName(string name)
        {
            return _beerRepository.GetAllByName(name);
        }

        public IEnumerable<Beer> GetByBrewery(string breweryId)
        {
            return _beerRepository.GetByBreweryId(breweryId);
        }

        public Beer GetById(string id)
        {
            return _beerRepository.GetById(id);
        }

        public string CreateBeer(string name, double abv, int baScore, string style, string color, string glass, string breweryId)
        {
            var id = string.Empty;

            using (var scope = new TransactionScope())
            {
                id = _beerRepository.NextIdentity();
                var beer = new Beer(id, name)
                {
                    ABV = abv,
                    BAScore = baScore,
                    BreweryId = breweryId,
                    Color = color,
                    Glass = glass,
                    Style = style,
                    RPScore = 0
                };
                _beerRepository.Add(beer);
                scope.Complete();
            }

            return id;
        }

        public IEnumerable<Beer> GetBeersByName(string name)
        {
            return _beerRepository.GetAllByName(name);
        }
    }
}
namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class KegOrchestrator : BaseOrchestrator, IKegOrchestrator
    {
        private readonly IKegRepository _kegRepository;

        private readonly ITapRepository _tapRepository;

        private readonly IBeerRepository _beerRepository;

        public KegOrchestrator(IKegRepository kegRepository, ITapRepository tapRepository, IBeerRepository beerRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");
            if (beerRepository == null) throw new ArgumentNullException("beerRepository");

            _kegRepository = kegRepository;
            _tapRepository = tapRepository;
            _beerRepository = beerRepository;
        }

        public IEnumerable<Keg> GetKegs()
        {
            return _kegRepository.GetAll();
        }

        public IEnumerable<Keg> GetKegs(bool isEmpty)
        {
            return _kegRepository.GetAll().Where(k => k.IsEmpty == isEmpty);
        } 

        public Keg GetKeg(string kegId)
        {
            return _kegRepository.GetById(kegId);
        }

        public IEnumerable<Keg> GetKegsOnTap()
        {
            var taps = _tapRepository.GetAll();
            var kegs = taps.Select(t => _kegRepository.GetById(t.KegId));

            return kegs;
        }

        public Keg GetKegOnTap(string tapId)
        {
            var tap = _tapRepository.GetById(tapId);
            var keg = _kegRepository.GetById(tap.KegId);

            return keg;
        }

        [Transactional]
        public string CreateKeg(string beerId, double capacity)
        {
            var id = _kegRepository.NextIdentity();
            var beer = _beerRepository.GetById(beerId);
            var keg = new Keg(id, beer.Id, capacity);

            _kegRepository.Add(keg);

            return id;
        }

        [Transactional]
        public void UpdateCapacityAndPoured(string kegId, double capacity, double amountOfBeerPoured)
        {
            var keg = _kegRepository.GetById(kegId);
            keg.UpdateCapacityAndPoured(capacity, amountOfBeerPoured);
            _kegRepository.Update(keg);
        }
    }
}
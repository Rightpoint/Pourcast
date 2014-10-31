namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Payloads;
    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class KegOrchestrator : BaseOrchestrator, IKegOrchestrator
    {
        private readonly IKegRepository _kegRepository;

        private readonly ITapRepository _tapRepository;

        private readonly IBeerRepository _beerRepository;

        private readonly IStoredEventRepository<PourStarted> _pourStartedRepository;

        private readonly IStoredEventRepository<PourStopped> _pourStoppedRepository;

        public KegOrchestrator(IKegRepository kegRepository, ITapRepository tapRepository, IBeerRepository beerRepository, IStoredEventRepository<PourStarted> pourStartedRepository, IStoredEventRepository<PourStopped> pourStoppedRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");
            if (beerRepository == null) throw new ArgumentNullException("beerRepository");
            if (pourStartedRepository == null) throw new ArgumentNullException("pourStartedRepository");
            if (pourStoppedRepository == null) throw new ArgumentNullException("pourStoppedRepository");

            _kegRepository = kegRepository;
            _tapRepository = tapRepository;
            _beerRepository = beerRepository;
            this._pourStartedRepository = pourStartedRepository;
            this._pourStoppedRepository = pourStoppedRepository;
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

        public IEnumerable<KegBurndownPoint> GetKegBurndown(string id)
        {
            var poursStopped = _pourStoppedRepository
                .Find(x => x.Event.KegId == id)
                .Select(x => new KegBurndownPoint(x.Event.PercentRemaining, x.OccuredOn));

            return poursStopped.OrderBy(x => x.OccurredOn);
        }
    }
}
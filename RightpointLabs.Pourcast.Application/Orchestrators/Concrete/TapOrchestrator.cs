namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class TapOrchestrator : ITapOrchestrator
    {
        private readonly ITapRepository _tapRepository;

        private readonly IKegRepository _kegRepository;

        public TapOrchestrator(ITapRepository tapRepository, IKegRepository kegRepository)
        {
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");

            _tapRepository = tapRepository;
            _kegRepository = kegRepository;
        }

        public Tap GetTapById(string id)
        {
            return _tapRepository.GetById(id);
        }

        public Tap GetByName(string name)
        {
            return _tapRepository.GetByName(name);
        }

        public IEnumerable<Tap> GetTaps()
        {
            return _tapRepository.GetAll();
        }

        [Transactional]
        public void StartPourFromTap(string tapId)
        {
            var tap = _tapRepository.GetById(tapId);
            var keg = _kegRepository.GetById(tap.KegId);

            keg.StartPourFromTap(tap.Id);

            _kegRepository.Update(keg);
        }

        [Transactional]
        public void StopPourFromTap(string tapId, double volume)
        {
            var tap = _tapRepository.GetById(tapId);
            var keg = _kegRepository.GetById(tap.KegId);

            keg.StopPourFromTap(tap.Id, volume);

            _kegRepository.Update(keg);
        }

        [Transactional]
        public void RemoveKegFromTap(string tapId)
        {
            var tap = _tapRepository.GetById(tapId);

            tap.RemoveKeg();

            _tapRepository.Update(tap);
        }

        [Transactional]
        public void TapKeg(string tapId, string kegId)
        {
            var tap = _tapRepository.GetById(tapId);
            var keg = _kegRepository.GetById(kegId);

            tap.TapKeg(keg.Id);

            _tapRepository.Update(tap);
        }

        [Transactional]
        public string CreateTap(string name)
        {
            var id = _tapRepository.NextIdentity();
            var tap = new Tap(id, name);
            _tapRepository.Add(tap);

            return id;
        }

        [Transactional]
        public string CreateTap(string name, string kegId)
        {
            var id = _tapRepository.NextIdentity();
            var tap = new Tap(id, name);
            _tapRepository.Add(tap);
            tap.TapKeg(kegId);
            return id;
        }

        [Transactional]
        public void Save(Tap tap)
        {
            _tapRepository.Update(tap);
        }
    }
}
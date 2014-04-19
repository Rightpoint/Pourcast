namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Transactions;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
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

        public IEnumerable<Tap> GetTaps()
        {
            return _tapRepository.GetAll();
        }

        public void StartPourFromTap(string tapId)
        {
            using (var scope = new TransactionScope())
            {
                var tap = _tapRepository.GetById(tapId);
                var keg = _kegRepository.GetById(tap.KegId);

                keg.StartPourFromTap(tap.Id);

                _kegRepository.Update(keg);

                scope.Complete();
            }
        }

        public void EndPourFromTap(string tapId, double volume)
        {
            using (var scope = new TransactionScope())
            {
                var tap = _tapRepository.GetById(tapId);
                var keg = _kegRepository.GetById(tap.KegId);

                keg.EndPourFromTap(tap.Id, volume);

                _kegRepository.Update(keg);

                scope.Complete();
            }
        }

        public void RemoveKegFromTap(string tapId)
        {
            using (var scope = new TransactionScope())
            {
                var tap = _tapRepository.GetById(tapId);

                tap.RemoveKeg();

                _tapRepository.Update(tap);

                scope.Complete();
            }
            
        }

        public void TapKeg(string tapId, string kegId)
        {
            using (var scope = new TransactionScope())
            {
                var tap = _tapRepository.GetById(tapId);
                var keg = _kegRepository.GetById(kegId);

                tap.TapKeg(keg.Id);

                _tapRepository.Update(tap);

                scope.Complete();
            }
        }

        public string CreateTap(TapName name)
        {
            var id = "";

            using (var scope = new TransactionScope())
            {
                id = _tapRepository.NextIdentity();
                var tap = new Tap(id, name);

                _tapRepository.Add(tap);

                scope.Complete();
            }

            return id;
        }

    }
}
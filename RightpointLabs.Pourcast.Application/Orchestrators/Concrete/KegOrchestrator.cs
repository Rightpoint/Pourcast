namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class KegOrchestrator : BaseOrchestrator, IKegOrchestrator
    {
        private readonly IKegRepository _kegRepository;

        private readonly ITapRepository _tapRepository;

        public KegOrchestrator(IKegRepository kegRepository, ITapRepository tapRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");

            _kegRepository = kegRepository;
            _tapRepository = tapRepository;
        }

        public IEnumerable<Keg> GetKegs()
        {
            return _kegRepository.GetAll();
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
    }
}
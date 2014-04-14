namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;

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

        public void PourBeerFromTap(string tapId, double volume)
        {
            var tap = _tapRepository.GetById(tapId);
            var keg = _kegRepository.GetById(tap.KegId);

            keg.PourBeerFromTap(tap.Id, volume);

            _kegRepository.Update(keg);
        }

        public void RemoveKegFromTap(string tapId)
        {
            var tap = _tapRepository.GetById(tapId);

            tap.RemoveKeg();

            _tapRepository.Update(tap);
        }

        public void TapKeg(string tapId, string kegId)
        {
            var tap = _tapRepository.GetById(tapId);
            var keg = _kegRepository.GetById(kegId);

            tap.TapKeg(keg.Id);

            _tapRepository.Update(tap);
        }

        public string CreateTap(TapName name)
        {
            var id = _tapRepository.NextIdentity();
            var tap = new Tap(id, name);

            _tapRepository.Add(tap);

            return id;
        }
    }
}
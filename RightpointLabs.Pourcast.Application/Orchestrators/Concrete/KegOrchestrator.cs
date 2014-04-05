namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;
    using System.Collections.Generic;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;

    public class KegOrchestrator : BaseOrchestrator, IKegOrchestrator
    {
        private readonly IKegRepository _kegRepository;

        private readonly IDateTimeProvider _dateTimeProvider;

        public KegOrchestrator(IKegRepository kegRepository, IDateTimeProvider dateTimeProvider)
        {
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");
            if (dateTimeProvider == null) throw new ArgumentNullException("dateTimeProvider");

            _kegRepository = kegRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public IEnumerable<Keg> GetKegs()
        {
            return _kegRepository.GetAll();
        }

        public IEnumerable<Keg> GetKegsOnTap()
        {
            return _kegRepository.OnTap();
        }

        public void PourBeerFromTap(string tapId, double volume)
        {
            var keg = _kegRepository.OnTap(tapId);
            var now = _dateTimeProvider.GetCurrentDateTime();
            
            keg.PourBeer(now, volume);

            _kegRepository.Update(keg);
        }
    }
}
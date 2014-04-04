namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;

    public class TapOrchestrator : ITapOrchestrator
    {
        private readonly IKegRepository _kegRepository;

        private readonly IDateTimeProvider _dateTimeProvider;

        public TapOrchestrator(IKegRepository kegRepository, IDateTimeProvider dateTimeProvider)
        {
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");
            if (dateTimeProvider == null) throw new ArgumentNullException("dateTimeProvider");

            _kegRepository = kegRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public void PourBeerFromTap(int tapId, double volume)
        {
            var newPour = new Pour(_dateTimeProvider.GetCurrentDateTime(), volume);
            var keg = _kegRepository.OnTap(tapId);

            keg.Pours.Add(newPour);

            // TODO : save it?
        }
    }

    public interface ITapRepository
    {
        Tap GetById(int id);
    }
}
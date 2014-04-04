namespace RightpointLabs.Pourcast.Application.Orchestrators.Concrete
{
    using System;

    using RightpointLabs.Pourcast.Application.Orchestrators.Abstract;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;

    public class TapOrchestrator : ITapOrchestrator
    {
        private readonly ITapRepository _tapRepository;

        private readonly IDateTimeProvider _dateTimeProvider;

        public TapOrchestrator(ITapRepository tapRepository, IDateTimeProvider dateTimeProvider)
        {
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");
            if (dateTimeProvider == null) throw new ArgumentNullException("dateTimeProvider");

            _tapRepository = tapRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public void PourBeerFromTap(string tapId, double volume)
        {
            var tap = _tapRepository.GetById(tapId);
            var currentTime = _dateTimeProvider.GetCurrentDateTime();
            
            tap.PourBeer(currentTime, volume);

            // TODO : save it?
        }
    }
}
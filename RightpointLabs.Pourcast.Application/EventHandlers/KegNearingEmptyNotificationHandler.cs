namespace RightpointLabs.Pourcast.Application.EventHandlers
{
    using System;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Repositories;

    public class KegNearingEmptyNotificationHandler : IEventHandler<BeerPoured>
    {
        private const double PercentageThreshold = 10;

        private readonly ITapRepository _tapRepository;

        private readonly IKegRepository _kegRepository;

        public KegNearingEmptyNotificationHandler(ITapRepository tapRepository, IKegRepository kegRepository)
        {
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");

            _tapRepository = tapRepository;
            _kegRepository = kegRepository;
        }

        public void Handle(BeerPoured domainEvent)
        {
            var kegId = domainEvent.KegId;
            var keg = _kegRepository.GetById(kegId);

            if (keg.PercentRemaining <= PercentageThreshold)
            {
                var tap = _tapRepository.GetByKegId(keg.Id);

                // todo : send notification
            }
        }
    }
}

namespace RightpointLabs.Pourcast.Application.EventHandlers
{
    using System;
    using System.Net.Mail;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;

    public class KegNearingEmptyNotificationHandler : TransactionDependentEventHandler<BeerPoured>
    {
        private const double PercentageThreshold = 10;

        private readonly IEmailService _emailService;

        private readonly ITapRepository _tapRepository;

        private readonly IKegRepository _kegRepository;

        public KegNearingEmptyNotificationHandler(ITapRepository tapRepository, IKegRepository kegRepository, IEmailService emailService)
        {
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");
            if (emailService == null) throw new ArgumentNullException("emailService");

            _tapRepository = tapRepository;
            _kegRepository = kegRepository;
            _emailService = emailService;
        }

        protected override void HandleAfterTransaction(BeerPoured domainEvent)
        {
            var kegId = domainEvent.KegId;
            var keg = _kegRepository.GetById(kegId);

            if (keg.PercentRemaining <= PercentageThreshold)
            {
                var tap = _tapRepository.GetByKegId(keg.Id);

                var notification = BuildNotification(domainEvent, tap, keg);
                _emailService.SendEmail(notification);
            }
        }

        private MailMessage BuildNotification(BeerPoured beerPoured, Tap tap, Keg keg)
        {
            // todo : build notification

            return null;
        }
    }
}

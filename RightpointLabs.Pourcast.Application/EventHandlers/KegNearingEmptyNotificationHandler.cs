namespace RightpointLabs.Pourcast.Application.EventHandlers
{
    using System;
    using System.Configuration;
    using System.Net.Mail;

    using RightpointLabs.Pourcast.Domain.Events;
    using RightpointLabs.Pourcast.Domain.Models;
    using RightpointLabs.Pourcast.Domain.Repositories;
    using RightpointLabs.Pourcast.Domain.Services;

    public class KegNearingEmptyNotificationHandler : IEventHandler<PourStopped>, IEventHandler<KegRemainingChanged>
    {
        private const double PercentageThreshold = .1;

        private readonly IEmailService _emailService;

        private readonly ITapRepository _tapRepository;

        private readonly IKegRepository _kegRepository;

        private readonly IBeerRepository _beerRepository;

        public KegNearingEmptyNotificationHandler(ITapRepository tapRepository, IKegRepository kegRepository, IEmailService emailService, IBeerRepository beerRepository)
        {
            if (tapRepository == null) throw new ArgumentNullException("tapRepository");
            if (kegRepository == null) throw new ArgumentNullException("kegRepository");
            if (emailService == null) throw new ArgumentNullException("emailService");
            if (beerRepository == null) throw new ArgumentNullException("beerRepository");

            _tapRepository = tapRepository;
            _kegRepository = kegRepository;
            _emailService = emailService;
            _beerRepository = beerRepository;
        }

        public void Handle(PourStopped domainEvent)
        {
            Handle(domainEvent.KegId);
        }

        public void Handle(KegRemainingChanged domainEvent)
        {
            Handle(domainEvent.KegId);
        }

        private void Handle(string kegId)
        {
            var keg = _kegRepository.GetById(kegId);

            if (keg.PercentRemaining <= PercentageThreshold)
            {
                var tap = _tapRepository.GetByKegId(keg.Id);

                var notification = BuildNotification(tap, keg);
                _emailService.SendEmail(notification);
            }
        }

        private MailMessage BuildNotification(Tap tap, Keg keg)
        {
            string emailaddy = ConfigurationManager.AppSettings.Get("EmailRecipient");
            Beer emptybeer = _beerRepository.GetById(keg.BeerId);
            double percentempty = (1 - keg.PercentRemaining) * 100;

            MailMessage message = new MailMessage();
            message.To.Add(emailaddy);
            message.IsBodyHtml = true;
            message.Subject = "The " + emptybeer.Name + " keg in tap #" + tap.Name + " is " + percentempty + "% empty";
            message.From = new MailAddress("Pourcast@rightpoint.com");
            message.Body = "<html><body><p>" +
                           "The " + emptybeer.Name + " keg in tap #" + tap.Name + " is " + percentempty + "% empty. " +
                           "</p>" +
                           "<p>If a new keg has not yet been ordered, get on that!</p>" +
                           "<p>Love,</p>" +
                           "<p>Your Pourcast</p>" +
                           "</body></html>";

            return message;
            
        }
    }
}

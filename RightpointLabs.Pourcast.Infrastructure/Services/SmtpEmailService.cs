using System;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    using System.Net.Mail;

    using RightpointLabs.Pourcast.Domain.Services;

    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _client;

        public SmtpEmailService(SmtpClient client)
        {
            if (client == null) throw new ArgumentNullException("client");

            _client = client;
        }

        public void SendEmail(MailMessage email)
        {
            if (email == null) return;

            _client.SendMailAsync(email);
        }
    }
}

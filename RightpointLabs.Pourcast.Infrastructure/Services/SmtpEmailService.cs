using System;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Transactions;

    using RightpointLabs.Pourcast.Domain.Services;

    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _client;

        private readonly Queue<MailMessage> _emailsToSend;

        public SmtpEmailService(SmtpClient client)
        {
            if (client == null) throw new ArgumentNullException("client");

            _client = client;
            _emailsToSend = new Queue<MailMessage>();
        }

        public void SendEmail(MailMessage email)
        {
            if (email == null) return;

            if (Transaction.Current == null)
            {
                _client.SendMailAsync(email);
            }
            else
            {
                _emailsToSend.Enqueue(email);
                Transaction.Current.TransactionCompleted += SendAllEmails;
            }
        }

        private void SendAllEmails(object sender, TransactionEventArgs e)
        {
            while (_emailsToSend.Any())
            {
                var email = _emailsToSend.Dequeue();
                _client.SendMailAsync(email);
            }
        }
    }
}

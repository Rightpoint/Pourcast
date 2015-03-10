using System;
using System.Reflection;
using log4net;

namespace RightpointLabs.Pourcast.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Net.Mail;

    using RightpointLabs.Pourcast.Application.Transactions;
    using RightpointLabs.Pourcast.Domain.Services;

    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _client;

        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SmtpEmailService(SmtpClient client)
        {
            if (client == null) throw new ArgumentNullException("client");

            _client = client;
        }

        public void SendEmail(MailMessage email)
        {
            if (email == null) return;

            TransactionExtensions.WaitForTransactionCompleted(() =>
            {
                try
                {
                    _client.Send(email);
                }
                catch (Exception ex)
                {
                    // failure to send the email should not bring everything crashing down
                    log.Warn("Unable to send email", ex);
                }
            });
        }
    }
}

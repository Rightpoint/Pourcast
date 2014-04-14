namespace RightpointLabs.Pourcast.Domain.Services
{
    using System.Net.Mail;

    public interface IEmailService
    {
        void SendEmail(MailMessage email);
    }
}

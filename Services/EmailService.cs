using CoderThoughtsBlog.Services.Interfaces;
using CoderThoughtsBlog.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Smime;
using System.Threading.Tasks;

namespace CoderThoughtsBlog.Services
{
    public class EmailService : IBlogEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage)
        {
            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(_mailSettings.Email ?? Environment.GetEnvironmentVariable("Email"));
            email.To.Add(MailboxAddress.Parse(_mailSettings.Email ?? Environment.GetEnvironmentVariable("Email")));
            email.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<b>{name}</b> has sent you an email and can be reached at: <b>{emailFrom}</b><br/><br/>{htmlMessage}";

            email.Body = builder.ToMessageBody();
            var port = _mailSettings.MailPort != 0 ? _mailSettings.MailPort : int.Parse(Environment.GetEnvironmentVariable("MailPort")!);

            using var smtp = new SmtpClient();
            smtp.Connect((_mailSettings.MailHost ?? Environment.GetEnvironmentVariable("MailHost")), (port), SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.MailPassword);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }

        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Email ?? Environment.GetEnvironmentVariable("Email"));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;

            //var builder = new BodyBuilder();
            //builder.HtmlBody = htmlMessage;
            var builder = new BodyBuilder()
            {
                HtmlBody = htmlMessage
            };

            var port = _mailSettings.MailPort != 0 ? _mailSettings.MailPort : int.Parse(Environment.GetEnvironmentVariable("MailPort")!);
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect((_mailSettings.MailHost ?? Environment.GetEnvironmentVariable("MailHost")), port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email ?? Environment.GetEnvironmentVariable("Email"), _mailSettings.MailPassword ?? Environment.GetEnvironmentVariable("MailPassword"));

            await smtp.SendAsync(email);

            smtp.Disconnect(true);

        }
    }
}

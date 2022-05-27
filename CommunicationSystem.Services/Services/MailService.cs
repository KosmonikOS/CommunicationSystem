using CommunicationSystem.Domain.Options;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Logging;

namespace CommunicationSystem.Services.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpOptions smtpOptions;
        private readonly MailOptions mailOptions;
        private readonly ILogger<MailService> logger;

        public MailService(IOptions<SmtpOptions> smtpOptions, IOptions<MailOptions> mailOptions
            , ILogger<MailService> logger)
        {
            this.smtpOptions = smtpOptions.Value;
            this.mailOptions = mailOptions.Value;
            this.logger = logger;
        }
        public async Task SendConfirmationAsync(string email, string token)
        {
            var subject = "Подтверждение регистрации";
            var body = $"Для подтверждения регистрации перейдите по" +
                $" <a href = '{mailOptions.RedirectUrl}/confirmation/{token}'>ссылке</a>";
            await SendMessage(email, subject, body);
            logger.LogInformation($"Confirmation letter is successfully sent to {email}");
        }

        public async Task SendRecoveredPasswordAsync(string email, string password)
        {
            var subject = "Временный пароль";
            var body = $"Ваш временный пароль: {password} <br/> " +
                $"Не забудьте сменить его";
            await SendMessage(email, subject, body);
            logger.LogInformation($"Recovery letter is successfully sent to {email}");
        }

        private async Task SendMessage(string email,string subject,string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("CommunicationSystem", smtpOptions.Login));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            emailMessage.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpOptions.Uri, 465, true);
                await client.AuthenticateAsync(smtpOptions.Login, smtpOptions.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}

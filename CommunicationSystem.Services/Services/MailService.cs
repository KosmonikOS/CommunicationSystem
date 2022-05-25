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
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("CommunicationSystem", smtpOptions.Login));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Подтверждение регистрации";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<span>Для подтверждения регистрации перейдите по <a href = '{mailOptions.RedirectUrl}/confirmation/{token}'>ссылке</a></span>";
            emailMessage.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpOptions.Uri, 465, true);
                await client.AuthenticateAsync(smtpOptions.Login, smtpOptions.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            logger.LogInformation($"Confirmation letter successfully sent to {email}");
        }
    }
}

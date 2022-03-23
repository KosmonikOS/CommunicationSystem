using CommunicationSystem.Options;
using CommunicationSystem.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Services
{
    public class MailService : IMailSender
    {
        private readonly SmtpOptions options;

        public MailService(IOptions<SmtpOptions> options)
        {
            this.options = options.Value;
        }
        public async Task SendRegistrationmail(string email,string token,string appurl)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("CommunicationSystem", options.Login));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Подтверждение регистрации";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<span>Для подтверждения регистрации перейдите по <a href = '{appurl}/api/registration/{token}'>ссылке</a></span>";
            emailMessage.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(options.Uri, 465, true);
                await client.AuthenticateAsync(options.Login,options.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

    }
}

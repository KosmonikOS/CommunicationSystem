using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Services
{
    public static class MailService
    {
        public static async Task SendRegistrationmail(string email,string token,string appurl)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("CommunicationSystem", "lapamydrosty@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Подтверждение регистрации";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<span>Для подтверждения регистрации перейдите по ссылке <a href = '{appurl}/api/registration/{token}'>{appurl}/api/registration/{token}</a></span>";
            emailMessage.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("lapamydrosty@gmail.com","JinsJins1");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

    }
}

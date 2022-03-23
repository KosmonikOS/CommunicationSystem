using System.Threading.Tasks;

namespace CommunicationSystem.Services.Interfaces
{
    public interface IMailSender
    {
        public Task SendRegistrationmailAsync(string email, string token, string appurl);
    }
}

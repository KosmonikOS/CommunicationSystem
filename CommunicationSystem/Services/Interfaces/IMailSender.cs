using System.Threading.Tasks;

namespace CommunicationSystem.Services.Interfaces
{
    public interface IMailSender
    {
        public Task SendRegistrationmail(string email, string token, string appurl);
    }
}

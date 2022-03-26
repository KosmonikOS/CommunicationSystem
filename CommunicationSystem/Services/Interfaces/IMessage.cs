using CommunicationSystem.Models;
using System.Threading.Tasks;

namespace CommunicationSystem.Services.Interfaces
{
    public interface IMessage
    {
        public Task SendMessage(Message message);
    }
}

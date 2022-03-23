using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CommunicationSystem.Services.Interfaces
{
    public interface IFileSaver
    {
        public Task<string> SaveFileAsync(IFormFile file);
    }
}

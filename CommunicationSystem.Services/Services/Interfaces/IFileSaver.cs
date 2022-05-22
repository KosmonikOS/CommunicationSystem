using Microsoft.AspNetCore.Http;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IFileSaver
    {
        public Task<string> SaveFileAsync(IFormFile file);
    }
}

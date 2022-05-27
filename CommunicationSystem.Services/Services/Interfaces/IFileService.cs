
using Microsoft.AspNetCore.Http;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile file);
    }
}

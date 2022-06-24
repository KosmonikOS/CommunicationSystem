using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CommunicationSystem.Services.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> SaveFileAsync(IFormFile file);
        public Task<FormValueProvider> SaveStreamFileWithFormDataAsync(HttpRequest request);
        public bool IsImage(string path);
    }
}

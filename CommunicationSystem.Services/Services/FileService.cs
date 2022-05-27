using CommunicationSystem.Domain.Options;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CommunicationSystem.Services.Services
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment env;
        private readonly PathOptions options;

        public FileService(IHostingEnvironment environment, IOptions<PathOptions> options)
        {
            this.env = environment;
            this.options = options.Value;
        }
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var path = options.AssetsFolder + DateTime.Now.TimeOfDay.TotalMilliseconds + file.FileName.ToString();
            using (var filestr = new FileStream(Path.Combine(env.ContentRootPath + options.AssetsPath + path), FileMode.Create))
            {
                await file.CopyToAsync(filestr);
            }
            return path;
        }
    }
}

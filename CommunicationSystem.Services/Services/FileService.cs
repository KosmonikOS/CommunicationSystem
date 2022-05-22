using CommunicationSystem.Domain.Options;
using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CommunicationSystem.Services.Services
{
    public class FileService : IFileSaver
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
            if (file != null)
            {
                try
                {
                    var path = options.AssetsFolder + DateTime.Now.TimeOfDay.TotalMilliseconds + file.FileName.ToString();
                    using (var filestr = new FileStream(Path.Combine(env.ContentRootPath + options.AssetsPath + path), FileMode.Create))
                    {
                        await file.CopyToAsync(filestr);
                    }
                    return path;

                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return null;
        }
    }
}

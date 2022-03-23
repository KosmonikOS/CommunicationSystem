using CommunicationSystem.Services.Interfaces;
using CommunicationSystem.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CommunicationSystem.Services
{
    public class FileService : IFileSaver
    {
        private readonly IWebHostEnvironment env;
        private readonly PathOptions options;

        public FileService(IWebHostEnvironment environment, IOptions<PathOptions> options)
        {
            this.env = environment;
            this.options = options.Value;
        }
        public async Task<string> SaveFileAsync(IFormFile file)
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
    }
}

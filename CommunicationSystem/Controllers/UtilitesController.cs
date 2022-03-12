using CommunicationSystem.Models;
using CommunicationSystem.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtilitesController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly PathOptions options;

        public UtilitesController(IWebHostEnvironment environment,IOptions<PathOptions> options)
        {
            env = environment;
            this.options = options.Value;
        }
        [HttpPut("saveimage")]
        public async Task<IActionResult> Put(IFormFile imageToSave)
        {
            if (imageToSave != null)
            {
                try
                {
                    var path = options.AssetsFolder + DateTime.Now.TimeOfDay.TotalMilliseconds + imageToSave.FileName.ToString();
                    using (var filestr = new FileStream(Path.Combine(env.ContentRootPath + options.AssetsPath + path), FileMode.Create))
                    {
                        await imageToSave.CopyToAsync(filestr);
                    }
                    return Ok(new { path = path});
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
    }
}

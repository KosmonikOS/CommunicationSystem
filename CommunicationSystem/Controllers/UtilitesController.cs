using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public UtilitesController(IWebHostEnvironment environment)
        {
            env = environment;
        }
        [HttpPut("saveimage")]
        public IActionResult Put(IFormFile imageToSave)
        {
            if (imageToSave != null)
            {
                try
                {
                    var path = "/assets/" + DateTime.Now.TimeOfDay.TotalMilliseconds + imageToSave.FileName.ToString();
                    //using (var filestr = new FileStream(env.ContentRootPath + "/ClientApp/src" + path, FileMode.Create))
                    //{
                        //await imageToSave.CopyToAsync(filestr);
                    //}
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

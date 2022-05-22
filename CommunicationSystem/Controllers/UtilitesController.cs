using CommunicationSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtilitesController : ControllerBase
    {
        private readonly IFileSaver file;

        public UtilitesController(IFileSaver file)
        {
            this.file = file;
        }
        [HttpPut("saveimage")]
        public async Task<IActionResult> Put(IFormFile imageToSave)
        {
            if (imageToSave != null)
            {
                try
                {
                    var path = await file.SaveFileAsync(imageToSave);
                    if (path != null)
                    {
                        return Ok(new { path = path });
                    }
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

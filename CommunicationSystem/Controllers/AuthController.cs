using CommunicationSystem.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("settime/{id}/{act?}")]
        public async Task<IActionResult> SetTime(int id, string act)
        {
            throw new Exception();
        }
        [HttpPost]
        public async Task<IActionResult> Enter(LoginDto dto)
        {
            throw new Exception();
        }
        //[HttpPost("refresh")]
        //public async Task<IActionResult> Refresh(TokenPair pair)
        //{
        //    throw new Exception();
        //}
    }
}

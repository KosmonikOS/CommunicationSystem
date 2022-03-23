﻿using CommunicationSystem.Models;
using CommunicationSystem.Options;
using CommunicationSystem.Repositories.Interfaces;
using CommunicationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        private readonly IJwtService jwt;

        public AuthController(IAuthRepository repository, IJwtService jwt)
        {
            this.repository = repository;
            this.jwt = jwt;
        }
        [HttpGet("settime/{id}/{act?}")]
        public async Task<IActionResult> SetTime(int id, string act)
        {
            if (id != 0)
            {
                try
                {
                    await repository.SetTimeAsync(id, act);
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Post(Login login)
        {
            try
            {
                var user = repository.GetConfirmedUser(login);
                if (user != null)
                {
                    var claims = jwt.GenerateClaims(user);
                    var token = jwt.GenerateJWT(claims);
                    var rt = await jwt.GenerateRTAsync(login.Email);
                    return Ok(
                        new
                        {
                            access_token = token,
                            refresh_token = rt,
                            current_account_id = user.Id
                        });
                }
                return Unauthorized();
            }
            catch(Exception e)
            {
                return Unauthorized(e);
            }
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenPair pair)
        {
            try
            {
                var result = await jwt.RefreshAsync(pair);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch(Exception e) 
            {
                return BadRequest(e);
            }
            return BadRequest();

        }
    }
}


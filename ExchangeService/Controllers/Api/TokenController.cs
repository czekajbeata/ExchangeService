using System.Collections.Generic;
using ExchangeService.Controllers.Logic;
using ExchangeService.Core;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
    //[Authorize]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;

        public TokenController(IJwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("api/token")]
        public IActionResult GenerateToken([FromBody] TokenViewModel tokenvm)
        {
            var token = _tokenService.BuildToken(tokenvm.Email);
            return Ok(new { token });
        }


    }
}
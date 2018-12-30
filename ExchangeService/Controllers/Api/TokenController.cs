using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
using ExchangeService.Core;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
    //[Authorize]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenController(IJwtTokenService tokenService, UserManager<IdentityUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }
        
        [Route("api/token")]
        public string GenerateToken(string email)
        {
            var token = _tokenService.BuildToken(email);
            return token;
        }

        [HttpPost("api/token/registration")]
        public async Task<IActionResult> Registration([FromBody] TokenViewModel tokenvm)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userManager.CreateAsync(new IdentityUser()
            {
                UserName = tokenvm.Email,
                Email = tokenvm.Email
            }, tokenvm.Password);

            if(!result.Succeeded)
            {
               return StatusCode(500);
            }
            return Ok();
        }

        [HttpPost("api/token/login")]
        public async Task<IActionResult> Login([FromBody] TokenViewModel tokenvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(tokenvm.Email);
            var correctUser = await _userManager.CheckPasswordAsync(user, tokenvm.Password);

            if(!correctUser)
            {
                return BadRequest("Username or password is incorrect");
            }

            return Ok(new { token = GenerateToken(tokenvm.Email) });
        }
    }
}
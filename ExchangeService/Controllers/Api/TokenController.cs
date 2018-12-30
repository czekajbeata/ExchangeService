using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
using ExchangeService.Core;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExchangeService.Controllers.Api
{
    //[Authorize]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public TokenController(IJwtTokenService tokenService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            this.signInManager = signInManager;
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

            var result = await signInManager.PasswordSignInAsync(tokenvm.Email,tokenvm.Password, false, lockoutOnFailure: true);
            
            if (!correctUser)
            {
                return BadRequest("Username or password is incorrect");
            }
            
            return Ok(new { token = GenerateToken(tokenvm.Email) });
        }
    }
}
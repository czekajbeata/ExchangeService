﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
using ExchangeService.Core;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfilesService profilesService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IJwtTokenService tokenService;
        private readonly SignInManager<IdentityUser> signInManager;

        public ProfilesController(ProfilesService profilesService, UserManager<IdentityUser> userManager, IJwtTokenService tokenService, SignInManager<IdentityUser> signInManager)
        {
            this.profilesService = profilesService;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
        }

        [HttpPost("api/users/profile")]
        public IActionResult AddUserProfile([FromBody] UserView user)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var result = profilesService.AddUserProfile(user, id);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpGet("api/users/{id?}")]
        public UserView GetUserProfile(int id)
        {
            return profilesService.GetUserProfile(id);
        }

       // [Authorize]
        [HttpGet("api/users/myprofile")]
        public UserView GetMyUserProfile()
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return profilesService.GetUserProfile(normalizedId);
        }

        [Route("api/token")]
        public string GenerateToken(string email)
        {
            var token = tokenService.BuildToken(email);
            return token;
        }

        [HttpPost("api/token/register")]
        public async Task<IActionResult> Register([FromBody] TokenViewModel tokenvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await userManager.CreateAsync(new IdentityUser()
            {
                UserName = tokenvm.Email,
                Email = tokenvm.Email
            }, tokenvm.Password);

            if (!result.Succeeded)
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

            var user = await userManager.FindByEmailAsync(tokenvm.Email);
            var correctUser = await userManager.CheckPasswordAsync(user, tokenvm.Password);

            var result = await signInManager.PasswordSignInAsync(tokenvm.Email, tokenvm.Password, false, lockoutOnFailure: true);

            if (!correctUser)
            {
                return BadRequest("Username or password is incorrect");
            }

            return Ok(new { token = GenerateToken(tokenvm.Email) });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
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
        
        public ProfilesController(ProfilesService profilesService, UserManager<IdentityUser> userManager)
        {
            this.profilesService = profilesService;
            this.userManager = userManager;
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

        [Authorize]
        [HttpGet("api/users/myprofile")]
        public UserView GetMyUserProfile()
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return profilesService.GetUserProfile(normalizedId);
        }
    }
}
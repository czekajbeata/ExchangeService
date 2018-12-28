using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfilesService profilesService;

        public ProfilesController(ProfilesService profilesService)
        {
            this.profilesService = profilesService;
        }

        [HttpPost("api/users/profile")]
        public IActionResult AddUserProfile([FromBody] UserView user)
        {
            string innerUserID = "1"; //only valid thing to receive from session
            var result = profilesService.AddUserProfile(user, innerUserID);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpGet("api/users/{id?}")]
        public UserView GetUserProfile(int id)
        {
            return profilesService.GetUserProfile(id);
        }

        [HttpGet("api/users/myprofile")]
        public UserView GetMyUserProfile()
        {
            string myInnerId = "1"; //TODO dodać wyciąganie z sesji
            var normalizedId = profilesService.ToNormalizedId(myInnerId);
            return profilesService.GetUserProfile(normalizedId);
        }
    }
}
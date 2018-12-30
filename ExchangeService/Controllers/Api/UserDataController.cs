using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly UserDataService userDataService;
        private readonly ProfilesService profilesService;

        public UserDataController(UserDataService userDataService, ProfilesService profilesService)
        {
            this.userDataService = userDataService;
            this.profilesService = profilesService;
        }

        [Authorize]
        [HttpPost("api/users/comments")]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            int currentUserId = 1; //TODO dodać wyciąganie z sesji
            comment.LeavingUserId = currentUserId;
            int receivingUserId = 2;

            var result = userDataService.AddComment(comment, receivingUserId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("api/users/exchanges")]
        public IActionResult AddExchange([FromBody] ExchangeDto exchange)
        {
            string myInnerId = "1"; //TODO dodać wyciąganie z sesji
            var normalizedId = profilesService.ToNormalizedId(myInnerId);
            var result = userDataService.AddExchange(exchange, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpGet("api/users/comments/{id?}")]
        public IEnumerable<CommentDto> GetComments(int id)
        {
            return userDataService.GetComments(id);
        }

        [Authorize]
        [HttpGet("api/users/mymatches")]
        public IEnumerable<MatchView> GetMatches()
        {
            string myInnerId = "1"; //TODO dodać wyciąganie z sesji
            var normalizedId = profilesService.ToNormalizedId(myInnerId);
            return userDataService.GetMatches(normalizedId);
        }

        [Authorize]
        [HttpGet("api/users/myexchanges")]
        public IEnumerable<ExchangeDto> GetMyExchanges()
        {
            string myInnerId = "1"; //TODO dodać wyciąganie z sesji
            var normalizedId = profilesService.ToNormalizedId(myInnerId);
            return userDataService.GetUserExchanges(normalizedId);
        }

        [HttpGet("api/users/exchanges/{id?}")]
        public IEnumerable<ExchangeDto> GetExchanges(int id)
        {
            return userDataService.GetUserExchanges(id);
        }
    }
}
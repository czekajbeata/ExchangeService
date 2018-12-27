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
    public class UsersController : ControllerBase
    {
        private readonly ProfilesService evaluatingService;

        public UsersController(ProfilesService evaluatingService)
        {
            this.evaluatingService = evaluatingService;
        }

        [HttpPost("api/users/comments")]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            int currentUserId = 1; //TODO dodać wyciąganie z sesji
            comment.LeavingUserId = currentUserId;
            int receivingUserId = 2;

            var result = evaluatingService.AddComment(comment, receivingUserId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPost("api/users/profile")]
        public IActionResult AddUserProfile([FromBody] UserDto user)
        {
            string innerUserID = "1"; //only valid thing to receive from session
            var result = evaluatingService.AddUserProfile(user, innerUserID);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpGet("api/users/comments/{id?}")]
        public IEnumerable<CommentDto> GetComments(int id)
        {
            return evaluatingService.GetComments(id);
        }

        [HttpGet("api/users/marks/{id?}")]
        public double GetAvgMark(int id)
        {
            return evaluatingService.GetAvgMark(id);
        }

        [HttpGet("api/users/matches")]
        public IEnumerable<MatchView> GetMatches()
        {
            string myInnerId = "1"; //TODO dodać wyciąganie z sesji
            var normalizedId = evaluatingService.ToNormalizedId(myInnerId);
            return evaluatingService.GetMatches(normalizedId);
        }

        [HttpGet("api/users/{id?}")]
        public UserDto GetUserProfile(int id)
        {
            return evaluatingService.GetUserProfile(id);
        }

        [HttpGet("api/users/myprofile")]
        public UserDto GetMyUserProfile()
        {
            string myInnerId = "1"; //TODO dodać wyciąganie z sesji
            var normalizedId = evaluatingService.ToNormalizedId(myInnerId);
            return evaluatingService.GetUserProfile(normalizedId);
        }

        [HttpGet("api/users/myexchanges")]
        public IEnumerable<ExchangeDto> GetMyExchanges()
        {
            string myInnerId = "1"; //TODO dodać wyciąganie z sesji
            var normalizedId = evaluatingService.ToNormalizedId(myInnerId);
            return evaluatingService.GetUserExchanges(normalizedId);
        }
    }
}
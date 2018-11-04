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
        private readonly EvaluatingService evaluatingService;

        public UsersController(EvaluatingService evaluatingService)
        {
            this.evaluatingService = evaluatingService;
        }

        [HttpPost("api/users/comments")]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            // if (!ModelState.IsValid)
             //   return BadRequest();

            int currentUserId = 1; //TODO dodać wyciąganie z sesji
            int receivingUserId = 2;

            var result = evaluatingService.AddComment(comment, currentUserId, receivingUserId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpGet("api/users/comments")]
        public IEnumerable<CommentDto> GetComments()
        {
            int userId = 2; //TODO dodać wyciąganie z sesji
            return evaluatingService.GetComments(userId);
        }

        [HttpGet("api/users/marks")]
        public double GetAvgMark()
        {
            int userId = 2; //TODO dodać wyciąganie z sesji
            return evaluatingService.GetAvgMark(userId);
        }

        [HttpGet("api/users/matches")]
        public IEnumerable<MatchView> GetMatches()
        {
            int userId = 1; //TODO dodać wyciąganie z sesji
            return evaluatingService.GetMatches(userId);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using ExchangeService.Controllers.Logic;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly DropDownService dropDownService;
        private readonly ShelvesService shelvesService;
        private readonly ProfilesService profilesService;

        public GamesController(DropDownService dropDownService, ShelvesService shelvesService, ProfilesService profilesService)
        {
            this.dropDownService = dropDownService;
            this.shelvesService = shelvesService;
            this.profilesService = profilesService;
        }
        
        [HttpGet("api/genres")]
        public IEnumerable<DropDownItem> GetGenres()
        {
            return dropDownService.GetGenres();
        }

        [HttpGet("api/games/{query?}")]
        public IEnumerable<DropDownItem> GetGames(string query)
        {
            return dropDownService.GetGamesList(query);
        }

        [HttpGet("api/games/get/{id}")]
        public IActionResult GetGameDetails(int id)
        {
            var game = shelvesService.GetGameDetails(id);

            if (game != null)
                return Ok(game);          

            return NotFound();
        }

        [HttpGet("api/users/searches/{id}")]
        public IEnumerable<UserSearchGameView> GetUserGameSearches(int id)
        {
            return shelvesService.GetUserSearchGames(id);
        }


        [HttpGet("api/users/search/{userSearchId}")]
        public UserSearchGameView GetUserSearch(int userSearchId)
        {
            return shelvesService.GetUserSearch(userSearchId);
        }


        [HttpGet("api/users/games/{userId}")]
        public IEnumerable<UserGameView> GetUserGames(int id)
        {
            return shelvesService.GetUserGames(id);
        }

        [HttpGet("api/usergames/{gameId}")]
        public IEnumerable<UserGameView> GetUserGamesByGame(int gameId)
        {
            return shelvesService.GetUserGamesByGame(gameId);
        }

        [HttpGet("api/users/game/{userGameId}")]
        public UserGameView GetUserGame(int userGameId)
        {
            return shelvesService.GetUserGame(userGameId);
        }

        [Authorize]
        [HttpPost("api/users/games")]
        public IActionResult AddUserGame([FromBody] UserGameDto userGameDto)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = shelvesService.AddUserGame(userGameDto, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("api/users/searches")]
        public IActionResult AddUserSearchGame([FromBody] UserSearchGameDto userGameDto)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = shelvesService.AddUserSearchGame(userGameDto, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("api/games")]
        public IActionResult AddGame([FromBody] GameDto gameDto)
        {
            var result = shelvesService.AddGame(gameDto);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPut("api/games")]
        public IActionResult UpdateGame([FromBody] GameDto gameDto)
        {
            var result = shelvesService.UpdateGame(gameDto);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPut("api/users/games")]
        public IActionResult UpdateUserGame([FromBody] UserGameDto userGameDto)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = shelvesService.UpdateUserGame(userGameDto, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpDelete("api/users/searches/{userSearchId}")]
        public IActionResult DeleteUserSearch(int userSearchId)
        {
            var result = shelvesService.DeleteUserSearch(userSearchId);
            return result ? (IActionResult)Ok() : NoContent();
        }

        [Authorize]
        [HttpDelete("api/users/games/{userGameId}")]
        public IActionResult DeleteUserGame(int userGameId)
        {
            var result = shelvesService.DeleteUserGame(userGameId);
            return result ? (IActionResult)Ok() : NoContent();
        }
    }
}
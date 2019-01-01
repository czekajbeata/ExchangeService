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

        [HttpGet("api/users/games/{id}")]
        public IEnumerable<UserGameView> GetUserGames(int id)
        {
            return shelvesService.GetUserGames(id);
        }

        [HttpGet("api/users/game/{id}")]
        public UserGameView GetUserGame(int id)
        {
            return shelvesService.GetUserGame(id);
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
    }
}
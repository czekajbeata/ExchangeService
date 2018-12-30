using System.Collections.Generic;
using ExchangeService.Controllers.Logic;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
   // [Authorize]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly DropDownService dropDownService;
        private readonly ShelvesService shelvesService;

        public GamesController(DropDownService dropDownService, ShelvesService shelvesService)
        {
            this.dropDownService = dropDownService;
            this.shelvesService = shelvesService;
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

        [HttpPost("api/users/games")]
        public IActionResult AddUserGame([FromBody] UserGameDto userGameDto)
        {
            int userId = 1; //TODO dodać wyciąganie z sesji

            var result = shelvesService.AddUserGame(userGameDto, userId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPost("api/users/searches")]
        public IActionResult AddUserSearchGame([FromBody] UserSearchGameDto userGameDto)
        {
            int userId = 1; //TODO dodać wyciąganie z sesji

            var result = shelvesService.AddUserSearchGame(userGameDto, userId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPost("api/games")]
        public IActionResult AddGame([FromBody] GameDto gameDto)
        {
            var result = shelvesService.AddGame(gameDto);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPut("api/games")]
        public IActionResult UpdateGame([FromBody] GameDto gameDto)
        {
            var result = shelvesService.UpdateGame(gameDto);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPut("api/users/games")]
        public IActionResult UpdateUserGame([FromBody] UserGameDto userGameDto)
        {
            int userId = 1; //TODO dodać wyciąganie z sesji

            var result = shelvesService.UpdateUserGame(userGameDto, userId);
            return result ? (IActionResult)Ok() : BadRequest();
        }
    }
}
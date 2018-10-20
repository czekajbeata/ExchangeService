using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
using ExchangeService.Controllers.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
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

        [HttpGet("api/games/{id}")]
        public IActionResult GetGameDetails(int id)
        {
            var game = shelvesService.GetGameDetails(id);

            if (game != null)
                return Ok(game);

            return NotFound();
        }

        [HttpGet("api/users/searches")]
        public IEnumerable<UserSearchGameView> GetUserGameSearches()
        {
            int userId = 1; //TODO dodać wyciąganie z sesji

            return shelvesService.GetUsersSearchGames(userId);
        }
        
        [HttpPost("api/users/games")]
        public IActionResult AddUserGame([FromBody] UserGameDto userGameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            int userId = 1; //TODO dodać wyciąganie z sesji

            var result = shelvesService.AddUserGame(userGameDto, userId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPost("api/users/searches")]
        public IActionResult AddUserSearchGame([FromBody] UserSearchGameDto userGameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            int userId = 1; //TODO dodać wyciąganie z sesji

            var result = shelvesService.AddUserSearchGame(userGameDto, userId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPost("api/games")]
        public IActionResult AddGame([FromBody] GameDto gameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = shelvesService.AddGame(gameDto);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPut("api/games")]
        public IActionResult UpdateGame([FromBody] GameDto gameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = shelvesService.UpdateGame(gameDto);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpPut("api/users/games")]
        public IActionResult UpdateUserGame([FromBody] UserGameDto userGameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            int userId = 1; //TODO dodać wyciąganie z sesji

            var result = shelvesService.UpdateUserGame(userGameDto, userId);
            return result ? (IActionResult)Ok() : BadRequest();
        }
    }
}
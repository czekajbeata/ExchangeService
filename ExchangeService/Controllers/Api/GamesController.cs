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
        private readonly GamesService gamesService;

        public GamesController(DropDownService dropDownService, GamesService gamesService)
        {
            this.dropDownService = dropDownService;
            this.gamesService = gamesService;
        }
        
        [HttpGet("api/genres")]
        public IEnumerable<DropDownItem> GetGenres()
        {
            return dropDownService.GetGenres();
        }

        [HttpGet("api/{query?}")]
        public IEnumerable<GameView> GetAllGames(string query)
        {
            return gamesService.GetAllGames(query);
        }

        [HttpGet("api/games/{query?}")]
        public IEnumerable<DropDownItem> GetGamesList(string query)
        {
            return dropDownService.GetGamesList(query);
        }

        [HttpGet("api/games/get/{id}")]
        public IActionResult GetGameDetails(int id)
        {
            var game = gamesService.GetGameDetails(id);

            if (game != null)
                return Ok(game);          

            return NotFound();
        }

        [HttpGet("api/games/getview/{id}")]
        public IActionResult GetGameView(int id)
        {
            var game = gamesService.GetGameView(id);

            if (game != null)
                return Ok(game);

            return NotFound();
        }

        [Authorize]
        [HttpPost("api/games")]
        public IActionResult AddGame([FromBody] GameDto gameDto)
        {
            var result = gamesService.AddGame(gameDto);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPut("api/games")]
        public IActionResult UpdateGame([FromBody] GameDto gameDto)
        {
            var result = gamesService.UpdateGame(gameDto);
            return result ? (IActionResult)Ok() : BadRequest();
        }
    }
}
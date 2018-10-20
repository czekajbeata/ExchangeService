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
    }
}
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

        public GamesController(DropDownService dropDownService)
        {
            this.dropDownService = dropDownService;
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
    }
}
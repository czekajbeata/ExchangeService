using ExchangeService.Controllers.Logic;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeService.Controllers.Api
{
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly ProfilesService profilesService;
        private readonly ExchangesService exchangeService;

        public UserDataController(ProfilesService profilesService, ExchangesService exchangeService)
        {
            this.profilesService = profilesService;
            this.exchangeService = exchangeService;
        }

        [HttpGet("api/users/comments/{id?}")]
        public IEnumerable<CommentDto> GetComments(int id)
        {
            return profilesService.GetComments(id);
        }

        [Authorize]
        [HttpGet("api/users/mymatches")]
        public IEnumerable<MatchAndUserView> GetMyMatches()
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return profilesService.GetMatches(normalizedId);
        }
        
        [Authorize]
        [HttpGet("api/users/myexchanges")]
        public IEnumerable<ExchangeView> GetMyExchanges()
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return exchangeService.GetMyExchanges(normalizedId);
        }

        [HttpGet("api/users/exchanges/{id?}")]
        public IEnumerable<ExchangeDto> GetExchanges(int id)
        {
            return exchangeService.GetUserExchanges(id);
        }

        [HttpGet("api/users/shortendexchange/{ExchangeId?}")]
        public ShortenedExchangeView GetShortenedExchange(int ExchangeId)
        {
            return exchangeService.GetShortenedExchange(ExchangeId);
        }

        [HttpGet("api/users/exchange/{ExchangeId?}")]
        public ExchangeDto GetExchange(int ExchangeId)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return exchangeService.GetExchange(ExchangeId, normalizedId);
        }

        [Authorize]
        [HttpPut("api/users/exchanges/decline")]
        public IActionResult AbandonExchange([FromBody] ShortenedExchangeView exchange)
        {
            var result = exchangeService.AbandonExchange(exchange.ExchangeId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPut("api/users/exchanges/accept")]
        public IActionResult AcceptExchange([FromBody] ExchangeDto exchange)
        {
            var result = exchangeService.AcceptExchange(exchange);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPut("api/users/exchanges/finalize")]
        public IActionResult FinalizeExchange([FromBody] ExchangeDto exchange)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = exchangeService.FinalizeExchange(exchange, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("api/users/comments")]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            var result = exchangeService.AddComment(comment);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("api/users/exchanges")]
        public IActionResult AddExchange([FromBody] ExchangeDto exchange)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = exchangeService.AddExchange(exchange, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }
    }
}
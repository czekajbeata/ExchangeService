using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
using ExchangeService.Shared.Enums;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
    [ApiController]
    public class UserGamesController : ControllerBase
    {
        private readonly ProfilesService profilesService;
        private readonly UserGamesService userGamesService;
        private readonly UserSearchesService userSearchesService;

        public UserGamesController(ProfilesService profilesService, UserGamesService userGamesService, UserSearchesService userSearchesService)
        {
            this.profilesService = profilesService;
            this.userGamesService = userGamesService;
            this.userSearchesService = userSearchesService;
        }

        [Authorize]
        [HttpGet("api/searches/check/{gameId}")]
        public bool CanAddSearch(int gameId)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return userSearchesService.CanAddSearch(gameId, normalizedId);
        }

        [HttpGet("api/users/search/{userSearchId}")]
        public UserSearchGameView GetUserSearch(int userSearchId)
        {
            return userSearchesService.GetUserSearch(userSearchId);
        }

        [HttpGet("api/users/searches/{id}")]
        public IEnumerable<UserSearchGameView> GetUserGameSearches(int id)
        {
            return userSearchesService.GetUserSearchGames(id);
        }

        [Authorize]
        [HttpGet("api/games/check/{gameId}")]
        public bool CanAddForExchange(int gameId)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return userGamesService.CanAddForExchange(gameId, normalizedId);
        }

        [HttpGet("api/users/game/{userGameId}")]
        public UserGameView GetUserGame(int userGameId)
        {
            return userGamesService.GetUserGame(userGameId);
        }

        [HttpGet("api/users/games/{userId}")]
        public IEnumerable<UserGameView> GetUserGames(int userId)
        {
            return userGamesService.GetUserGames(userId);
        }

        [HttpGet("api/usergames/{gameId}")]
        public IEnumerable<GameAndUserView> GetUserGamesByGame(int gameId)
        {
            return userGamesService.GetUserGamesByGame(gameId);
        }
        
        [Authorize]
        [HttpPost("api/users/searches")]
        public IActionResult AddUserSearchGame([FromBody] UserSearchGameDto userGameDto)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = userSearchesService.AddUserSearchGame(userGameDto, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("api/users/games")]
        public IActionResult AddUserGame([FromBody] UserGameDto userGameDto)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = userGamesService.AddUserGame(userGameDto, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        
        [Authorize]
        [HttpPut("api/users/games")]
        public IActionResult UpdateUserGame([FromBody] UserGameDto userGameDto)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = userGamesService.UpdateUserGame(userGameDto, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpDelete("api/users/searches/{userSearchId}")]
        public IActionResult DeleteUserSearch(int userSearchId)
        {
            var result = userSearchesService.DeleteUserSearch(userSearchId);
            return result ? (IActionResult)Ok() : NoContent();
        }

        [Authorize]
        [HttpDelete("api/users/games/{userGameId}")]
        public IActionResult DeleteUserGame(int userGameId)
        {
            var result = userGamesService.DeleteUserGame(userGameId);
            return result ? (IActionResult)Ok() : NoContent();
        }
    }
}
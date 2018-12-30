﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeService.Controllers.Logic;
using ExchangeService.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeService.Controllers.Api
{
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly UserDataService userDataService;
        private readonly ProfilesService profilesService;

        public UserDataController(UserDataService userDataService, ProfilesService profilesService)
        {
            this.userDataService = userDataService;
            this.profilesService = profilesService;
        }

        [Authorize]
        [HttpPost("api/users/comments")]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            comment.LeavingUserId = normalizedId;
            int receivingUserId = 2;

            var result = userDataService.AddComment(comment, receivingUserId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [Authorize]
        [HttpPost("api/users/exchanges")]
        public IActionResult AddExchange([FromBody] ExchangeDto exchange)
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            var result = userDataService.AddExchange(exchange, normalizedId);
            return result ? (IActionResult)Ok() : BadRequest();
        }

        [HttpGet("api/users/comments/{id?}")]
        public IEnumerable<CommentDto> GetComments(int id)
        {
            return userDataService.GetComments(id);
        }

        [Authorize]
        [HttpGet("api/users/mymatches")]
        public IEnumerable<MatchView> GetMyMatches()
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return userDataService.GetMatches(normalizedId);
        }

        [Authorize]
        [HttpGet("api/users/myexchanges")]
        public IEnumerable<ExchangeDto> GetMyExchanges()
        {
            var id = User.Claims.Single(c => c.Type == "Id").Value;
            var normalizedId = profilesService.ToNormalizedId(id);
            return userDataService.GetUserExchanges(normalizedId);
        }

        [HttpGet("api/users/exchanges/{id?}")]
        public IEnumerable<ExchangeDto> GetExchanges(int id)
        {
            return userDataService.GetUserExchanges(id);
        }
    }
}
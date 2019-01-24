using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ExchangeService.Controllers.Logic
{
    public class UserSearchesService
    {
        private readonly IGames games;
        private readonly IUserSearches userSearches;
        private readonly IUserProfiles userProfiles;
        private readonly IUnitOfWork unitOfWork;

        public UserSearchesService(IGames games, IUserSearches userSearches, IUserProfiles userProfiles, IUnitOfWork unitOfWork)
        {
            this.games = games;
            this.userSearches = userSearches;
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
        }

        public bool CanAddSearch(int gameId, int userId)
        {
            var userSearch = userSearches.GetUserSearch(gameId, userId);
            return userSearch == null;
        }

        public bool AddUserSearchGame(UserSearchGameDto newUserSearchGame, int userId)
        {
            UserSearchGame newUserSearch = new UserSearchGame()
            {
                UserId = userId,
                GameId = newUserSearchGame.GameId
            };
            userSearches.AddSearchGame(newUserSearch);
            unitOfWork.CompleteWork();
            return newUserSearch.UserSearchGameId != 0;
        }

        public UserSearchGameView GetUserSearch(int userSearchId)
        {
            var userSearch = userSearches.GetUserSearch(userSearchId);
            var game = games.GetGame(userSearch.GameId);
            return new UserSearchGameView()
            {
                UserSearchId = userSearch.UserSearchGameId,
                GameId = userSearch.GameId,
                UserId = userSearch.UserId,
                ImageUrl = game.ImageUrl,
                Title = game.Title
            };
        }

        public IEnumerable<UserSearchGameView> GetUserSearchGames(int userId)
        {
            var searchedGames = userSearches.GetUserSearchGames(userId);
            List<UserSearchGameView> searchedGameViews = new List<UserSearchGameView>();
            foreach (var game in searchedGames)
            {
                var gamePiece = GetGameDetails(game.GameId);
                searchedGameViews.Add(new UserSearchGameView()
                {
                    UserSearchId = game.UserSearchGameId,
                    GameId = game.GameId,
                    Title = gamePiece.Title,
                    ImageUrl = gamePiece.ImageUrl
                });
            }
            return searchedGameViews;
        }

        public bool DeleteUserSearch(int userSearchId)
        {
            bool result = userSearches.DeleteUserSearch(userSearchId);
            unitOfWork.CompleteWork();
            return result;
        }
        
        private GameDto GetGameDetails(int id)
        {
            var game = games.GetGame(id);
            if (game is null)
                return null;
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            return new GameDto()
            {
                Description = game.Description,
                GameId = game.GameId,
                GenreId = game.GenreId,
                ImageUrl = game.ImageUrl,
                PlayerCount = playerCount,
                MinAgeRequired = game.MinAgeRequired.ToString(),
                Publisher = game.Publisher,
                Title = game.Title,
                GameTimeInMin = game.GameTimeInMin.ToString()
            };
        }
    }
}

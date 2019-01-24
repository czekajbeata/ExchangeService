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
        private readonly MappingService mappingService;

        public UserSearchesService(IGames games, IUserSearches userSearches, IUserProfiles userProfiles, IUnitOfWork unitOfWork, MappingService mappingService)
        {
            this.games = games;
            this.userSearches = userSearches;
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
            this.mappingService = mappingService;
        }

        public bool CanAddSearch(int gameId, int userId)
        {
            var userSearch = userSearches.GetUserSearch(gameId, userId);
            return userSearch == null;
        }

        public bool AddUserSearchGame(UserSearchGameDto newUserSearchGame, int userId)
        {
            UserSearchGame newUserSearch = mappingService.GetUserSearchGameFromDto(newUserSearchGame, userId);
            userSearches.AddSearchGame(newUserSearch);
            unitOfWork.CompleteWork();
            return newUserSearch.UserSearchGameId != 0;
        }

        public UserSearchGameView GetUserSearch(int userSearchId)
        {
            var userSearch = userSearches.GetUserSearch(userSearchId);
            var game = games.GetGame(userSearch.GameId);
            return mappingService.GetUserSearchGameView(game, userSearch);
        }

        public IEnumerable<UserSearchGameView> GetUserSearchGames(int userId)
        {
            var searchedGames = userSearches.GetUserSearchGames(userId);
            List<UserSearchGameView> searchedGameViews = new List<UserSearchGameView>();
            foreach (var game in searchedGames)
            {
                var gamePiece = games.GetGame(game.GameId);
                searchedGameViews.Add(mappingService.GetUserSearchGameView(gamePiece, game));
            }
            return searchedGameViews;
        }

        public bool DeleteUserSearch(int userSearchId)
        {
            bool result = userSearches.DeleteUserSearch(userSearchId);
            unitOfWork.CompleteWork();
            return result;
        }
    }
}

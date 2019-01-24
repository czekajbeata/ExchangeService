using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ExchangeService.Controllers.Logic
{
    public class UserGamesService
    {
        private readonly IGames games;
        private readonly IUserGames userGames;
        private readonly IUserProfiles userProfiles;
        private readonly IUnitOfWork unitOfWork;
        private readonly MappingService mappingService;

        public UserGamesService(IGames games, IUserGames userGames, IUserProfiles userProfiles, IUnitOfWork unitOfWork, MappingService mappingService)
        {
            this.games = games;
            this.userGames = userGames;
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
            this.mappingService = mappingService;
        }

        public bool AddUserGame(UserGameDto newUserGameDto, int userId)
        {
            UserGame newUserGame = mappingService.GetUserGameFromDto(newUserGameDto, userId);
            userGames.AddGame(newUserGame);
            unitOfWork.CompleteWork();
            return newUserGame.GameId != 0;
        }
        
        public bool CanAddForExchange(int gameId, int userId)
        {
            var userGame = userGames.GetUserGame(gameId, userId);
            return userGame == null;
        }

        public UserGameDto GetUserGame(int userGameId)
        {
            var usergame = userGames.GetUserGame(userGameId);
            return mappingService.GetUserGameDtoFromUserGame(usergame);
        }

        public UserGameView GetUserGame(int gameId, int userId)
        {
            var usergame = userGames.GetUserGame(gameId, userId);
            var game = games.GetGame(usergame.GameId);
            var genre = games.GetGenre(game.GenreId).Name;
            return mappingService.GetUserGameView(game, genre, usergame);
        }

        public bool DeleteUserGame(int userGameId)
        {
            bool result = userGames.DeleteUserGame(userGameId);
            unitOfWork.CompleteWork();
            return result;
        }
        
        public bool UpdateUserGame(UserGameDto updatedGame, int userId)
        {
            var existingGame = userGames.GetUserGame(updatedGame.GameId, userId);
            if (existingGame == null)
                return false;

            existingGame.State = updatedGame.State;
            existingGame.IsComplete = updatedGame.IsComplete;
            existingGame.Shipment = updatedGame.Shipment;
            existingGame.UserGameDescription = updatedGame.UserGameDescription;
            string images = updatedGame.UserImages.Any()
                ? string.Join(",", updatedGame.UserImages)
                : string.Empty;
            existingGame.UserGameImages = images;

            unitOfWork.CompleteWork();
            return true;
        }

        public IEnumerable<UserGameView> GetUserGames(int userId)
        {
            var gamePieces = userGames.GetUserGames(userId);
            List<UserGameView> gameViews = new List<UserGameView>();
            foreach (var game in gamePieces)
            {
                var gameCopy = games.GetGame(game.GameId);
                var genre = games.GetGenre(gameCopy.GenreId).Name;
                gameViews.Add(mappingService.GetUserGameView(gameCopy, genre, game));
            }
            return gameViews;
        }

        public IEnumerable<GameAndUserView> GetUserGamesByGame(int gameId)
        {
            var gamePieces = userGames.GetUserGamesByGame(gameId);
            List<GameAndUserView> gameViews = new List<GameAndUserView>();
            foreach (var game in gamePieces)
            {
                var userProfile = userProfiles.GetUserProfile(game.UserId);
                var comments = userProfiles.GetComments(game.UserId);
                var gameCopy = games.GetGame(game.GameId);         
                gameViews.Add(mappingService.GetGameAndUserView(gameCopy, game, userProfile, comments));
            }
            return gameViews;
        }
    }
}

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

        public UserGamesService(IGames games, IUserGames userGames, IUserProfiles userProfiles, IUnitOfWork unitOfWork)
        {
            this.games = games;
            this.userGames = userGames;
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
        }

        public bool AddUserGame(UserGameDto newUserGameDto, int userId)
        {
            string images = newUserGameDto.UserImages.Count() > 1
                ? string.Join(",", newUserGameDto.UserImages)
                : newUserGameDto.UserImages.Count() == 0
                ? string.Empty
                : newUserGameDto.UserImages[0];
            UserGame newUserGame = new UserGame()
            {
                UserId = userId,
                GameId = newUserGameDto.GameId,
                IsComplete = newUserGameDto.IsComplete,
                Shipment = newUserGameDto.Shipment,
                State = newUserGameDto.State,
                UserGameDescription = newUserGameDto.UserGameDescription,
                UserGameImages = images
            };
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
            string[] images = string.IsNullOrEmpty(usergame.UserGameImages)
            ? new string[] { }
            : usergame.UserGameImages.Contains(',')
            ? usergame.UserGameImages.Split(',')
            : new string[] { usergame.UserGameImages };
            return new UserGameDto()
            {
                GameId = usergame.GameId,
                UserId = usergame.UserId,
                IsComplete = usergame.IsComplete,
                Shipment = usergame.Shipment,
                State = usergame.State,
                UserGameDescription = usergame.UserGameDescription,
                UserImages = images
            };
        }

        public UserGameView GetUserGame(int gameId, int userId)
        {
            var usergame = userGames.GetUserGame(gameId, userId);
            var game = games.GetGame(usergame.GameId);
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            string[] images = string.IsNullOrEmpty(usergame.UserGameImages)
            ? new string[] { }
            : usergame.UserGameImages.Contains(',')
            ? usergame.UserGameImages.Split(',')
            : new string[] { usergame.UserGameImages };
            return new UserGameView()
            {
                UserGameId = usergame.UserGameId,
                GameId = usergame.GameId,
                UserId = usergame.UserId,
                GameTimeInMin = game.GameTimeInMin.ToString(),
                Description = game.Description,
                GenreName = games.GetGenre(game.GenreId).Name,
                ImageUrl = game.ImageUrl,
                IsComplete = usergame.IsComplete,
                MinAgeRequired = game.MinAgeRequired.ToString(),
                PlayerCount = playerCount,
                Publisher = game.Publisher ?? String.Empty,
                Shipment = usergame.Shipment,
                State = usergame.State,
                Title = game.Title,
                UserGameDescription = usergame.UserGameDescription,
                UserImages = images
            };
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
            string images = updatedGame.UserImages.Count() > 1
            ? string.Join(",", updatedGame.UserImages)
            : updatedGame.UserImages.Count() == 0
            ? string.Empty
            : updatedGame.UserImages[0];
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
                var gameCopy = GetGameDetails(game.GameId);
                string[] images = string.IsNullOrEmpty(game.UserGameImages)
                    ? new string[] { } 
                    : game.UserGameImages.Contains(',')
                    ? game.UserGameImages.Split(',')
                    : new string[] { game.UserGameImages };

                gameViews.Add(new UserGameView()
                {
                    UserGameId = game.UserGameId,
                    GameId = game.GameId,
                    UserId = game.UserId,
                    Title = gameCopy.Title,
                    Description = gameCopy.Description,
                    UserGameDescription = game.UserGameDescription,
                    ImageUrl = gameCopy.ImageUrl,
                    Publisher = gameCopy.Publisher,
                    GenreName = games.GetGenre(gameCopy.GenreId).Name,
                    PlayerCount = gameCopy.PlayerCount,
                    MinAgeRequired = gameCopy.MinAgeRequired,
                    State = game.State,
                    IsComplete = game.IsComplete,
                    Shipment = game.Shipment,
                    GameTimeInMin = gameCopy.GameTimeInMin,
                    UserImages = images
                });
            }
            return gameViews;
        }

        public IEnumerable<GameAndUserView> GetUserGamesByGame(int gameId)
        {
            var gamePieces = userGames.GetUserGamesByGame(gameId);
            List<GameAndUserView> gameViews = new List<GameAndUserView>();
            foreach (var game in gamePieces)
            {
                var gameCopy = GetGameDetails(game.GameId);
                var userProfile = userProfiles.GetUserProfile(game.UserId);
                var comments = userProfiles.GetComments(game.UserId);
                var avgMark = userProfiles.GetComments(game.UserId).Select(c => c.Mark).Sum() / comments.Count();
                if (!(avgMark > 0)) avgMark = 0;
                else avgMark = Math.Round(avgMark, 2);
                gameViews.Add(new GameAndUserView()
                {
                    UserGameId = game.UserGameId,
                    GameId = game.GameId,
                    UserId = game.UserId,
                    Title = gameCopy.Title,
                    PlayerCount = gameCopy.PlayerCount,
                    MinAgeRequired = gameCopy.MinAgeRequired,
                    GameTimeInMin = gameCopy.GameTimeInMin,
                    State = game.State,
                    IsComplete = game.IsComplete,
                    Shipment = game.Shipment,
                    ImageUrl = gameCopy.ImageUrl,
                    Name = userProfile.Name,
                    Surname = userProfile.Surname,
                    Location = userProfile.Location,
                    UserImageUrl = userProfile.ImageUrl,
                    AvgMark = avgMark
                });
            }
            return gameViews;
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

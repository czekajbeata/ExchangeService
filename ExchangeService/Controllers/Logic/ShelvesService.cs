using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ExchangeService.Controllers.Logic
{
    public class ShelvesService
    {
        private readonly IGames games;
        private readonly IUserProfiles userProfiles;
        private readonly IUnitOfWork unitOfWork;

        public ShelvesService(IGames games, IUserProfiles userProfiles, IUnitOfWork unitOfWork)
        {
            this.games = games;
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
        }

        public bool AddUserGame(UserGameDto newUserGameDto, int userId)
        {
            UserGame newUserGame = new UserGame()
            {
                UserId = userId,
                GameId = newUserGameDto.GameId,
                IsComplete = newUserGameDto.IsComplete,
                Shipment = newUserGameDto.Shipment,
                State = newUserGameDto.State,
                UserGameDescription = newUserGameDto.UserGameDescription
            };
            userProfiles.AddGame(newUserGame);
            unitOfWork.CompleteWork();
            return newUserGame.GameId != 0;
        }

        public IEnumerable<GameDto> GetAllGames(string query)
        {
            var gamesDetails = games.GetGames(query);
            List<GameDto> gameDtos = new List<GameDto>();
            foreach(var game in gamesDetails)
            {
                var genreName = game.GenreId != null ? games.GetGenre(game.GenreId).Name : null;
                var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
                gameDtos.Add(new GameDto()
                {
                    Description = game.Description,
                    GameId = game.GameId,
                    GenreName = genreName,
                    ImageUrl = game.ImageUrl,
                    PlayerCount = playerCount,
                    MinAgeRequired = game.MinAgeRequired.ToString(),
                    Publisher = game.Publisher,
                    Title = game.Title,
                    GameTimeInMin = game.GameTimeInMin.ToString()
                });
            }
            return gameDtos;
        }

        public bool AddGame(GameDto game)
        {
            Game newGame = new Game()
            {
                Description = game.Description ?? String.Empty,
                GenreId =  game.GenreName != null ? games.GetGenreByName(game.GenreName).GenreId : (int?)null,
                ImageUrl = game.ImageUrl ?? null,
                MaxPlayerCount = games.GetPlayerCounts(game.PlayerCount).Item2,
                MinPlayerCount = games.GetPlayerCounts(game.PlayerCount).Item1,
                MinAgeRequired = game.MinAgeRequired != null ? Int32.Parse(game.MinAgeRequired) : (int?)null,
                Publisher = game.Publisher ?? String.Empty,
                Title = game.Title ?? String.Empty,
                GameTimeInMin = game.GameTimeInMin != null ?  Int32.Parse(game.GameTimeInMin) : (int?)null
            };
            games.AddGame(newGame);
            unitOfWork.CompleteWork();
            return newGame.GameId != 0;
        }

        public UserSearchGameView GetUserSearch(int userSearchId)
        {
            var userSearch = userProfiles.GetUserSearch(userSearchId);
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

        public UserGameView GetUserGame(int userGameId)
        {
            var usergame = userProfiles.GetUserGame(userGameId);
            var game = games.GetGame(usergame.GameId);
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            return new UserGameView()
            {
                UserGameId = usergame.UserGameId,
                GameId = usergame.GameId,
                UserId = usergame.UserId,
                GameTimeInMin = game.GameTimeInMin.ToString() ?? String.Empty,
                Description = game.Description,
                GenreName = game.GenreId != null ? games.GetGenre(game.GenreId).Name ?? String.Empty : String.Empty,
                ImageUrl = game.ImageUrl ?? String.Empty,
                IsComplete = usergame.IsComplete,
                MinAgeRequired = game.MinAgeRequired.ToString() ?? String.Empty,
                PlayerCount = playerCount ?? String.Empty,
                Publisher = game.Publisher ?? String.Empty,
                Shipment = usergame.Shipment,
                State = usergame.State,
                Title = game.Title,
                UserGameDescription = usergame.UserGameDescription ?? String.Empty
            };
        }

        public UserGameView GetUserGame(int gameId, int userId)
        {
            var usergame = userProfiles.GetUserGame(gameId, userId);
            var game = games.GetGame(usergame.GameId);
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            return new UserGameView()
            {
                UserGameId = usergame.UserGameId,
                GameId = usergame.GameId,
                UserId = usergame.UserId,
                GameTimeInMin = game.GameTimeInMin.ToString() ?? String.Empty,
                Description = game.Description,
                GenreName = game.GenreId != null ? games.GetGenre(game.GenreId).Name ?? String.Empty : String.Empty,
                ImageUrl = game.ImageUrl ?? String.Empty,
                IsComplete = usergame.IsComplete,
                MinAgeRequired = game.MinAgeRequired.ToString() ?? String.Empty,
                PlayerCount = playerCount ?? String.Empty,
                Publisher = game.Publisher ?? String.Empty,
                Shipment = usergame.Shipment,
                State = usergame.State,
                Title = game.Title,
                UserGameDescription = usergame.UserGameDescription ?? String.Empty
            };
        }

        public bool DeleteUserGame(int userGameId)
        {
            bool result = userProfiles.DeleteUserGame(userGameId);
            unitOfWork.CompleteWork();
            return result;
        }

        public bool DeleteUserSearch(int userSearchId)
        {
            bool result = userProfiles.DeleteUserSearch(userSearchId);
            unitOfWork.CompleteWork();
            return result;
        }

        public bool AddUserSearchGame(UserSearchGameDto newUserSearchGame, int userId)
        {
            UserSearchGame newUserSearch = new UserSearchGame()
            {
                UserId = userId,
                GameId = newUserSearchGame.GameId
            };
            userProfiles.AddSearchGame(newUserSearch);
            unitOfWork.CompleteWork();
            return newUserSearch.UserSearchGameId != 0;
        }

        public GameDto GetGameDetails(int id)
        {
            var game = games.GetGame(id);

            if (game is null)
                return null;
            var genreName = game.GenreId != null ? games.GetGenre(game.GenreId).Name : null;
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            return new GameDto()
            {
                Description = game.Description,
                GameId = game.GameId,
                GenreName = genreName,
                ImageUrl = game.ImageUrl,
                PlayerCount = playerCount,
                MinAgeRequired = game.MinAgeRequired.ToString(),
                Publisher = game.Publisher,
                Title = game.Title,
                GameTimeInMin = game.GameTimeInMin.ToString()
            };
        }

        public bool UpdateGame(GameDto updatedGame)
        {
            var existingGame = games.GetGame(updatedGame.GameId);
            if (existingGame == null)
                return false;

            existingGame.Description = updatedGame.Description;
            existingGame.GenreId = updatedGame.GenreName != null ? games.GetGenreByName(updatedGame.GenreName).GenreId : (int?)null;
            existingGame.ImageUrl = updatedGame.ImageUrl;
            existingGame.MaxPlayerCount = games.GetPlayerCounts(updatedGame.PlayerCount).Item2;
            existingGame.MinPlayerCount = games.GetPlayerCounts(updatedGame.PlayerCount).Item1;
            existingGame.MinAgeRequired = Int32.Parse(updatedGame.MinAgeRequired);
            existingGame.Publisher = updatedGame.Publisher;
            existingGame.Title = updatedGame.Title;
            existingGame.GameTimeInMin = Int32.Parse(updatedGame.GameTimeInMin);

            unitOfWork.CompleteWork();
            return true;
        }

        public bool UpdateUserGame(UserGameDto updatedGame, int userId)
        {
            var existingGame = userProfiles.GetUserGame(updatedGame.GameId, userId);
            if (existingGame == null)
                return false;

            existingGame.State = updatedGame.State;
            existingGame.IsComplete = updatedGame.IsComplete;
            existingGame.Shipment = updatedGame.Shipment;
            existingGame.UserGameDescription = updatedGame.UserGameDescription;

            unitOfWork.CompleteWork();
            return true;
        }

        public IEnumerable<UserSearchGameView> GetUserSearchGames(int userId)
        {
            var searchedGames = userProfiles.GetUserSearchGames(userId);
            List<UserSearchGameView> searchedGameViews = new List<UserSearchGameView>();
            foreach(var game in searchedGames)
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

        public IEnumerable<UserGameView> GetUserGames(int userId)
        {
            var gamePieces = userProfiles.GetUserGames(userId);
            List<UserGameView> gameViews = new List<UserGameView>();
            foreach (var game in gamePieces)
            {
                var gameCopy = GetGameDetails(game.GameId);
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
                    GenreName = gameCopy.GenreName,
                    PlayerCount = gameCopy.PlayerCount,
                    MinAgeRequired = gameCopy.MinAgeRequired,
                    State = game.State,
                    IsComplete = game.IsComplete,
                    Shipment = game.Shipment,
                    GameTimeInMin = gameCopy.GameTimeInMin
                });
            }
            return gameViews;
        }

        public IEnumerable<GameAndUserView> GetUserGamesByGame(int gameId)
        {
            var gamePieces = userProfiles.GetUserGamesByGame(gameId);
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
        
    }
}

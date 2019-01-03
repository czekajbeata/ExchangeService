using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System.Collections.Generic;
using System;

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
                PublishDate = game.PublishDate,
                Publisher = game.Publisher ?? String.Empty,
                Title = game.Title ?? String.Empty
            };
            games.AddGame(newGame);
            unitOfWork.CompleteWork();
            return newGame.GameId != 0;
        }

        internal UserSearchGameView GetUserSearch(int userSearchId)
        {
            var userSearch = userProfiles.GetUserSearch(userSearchId);
            var game = games.GetGame(userSearch.GameId);
            return new UserSearchGameView()
            {
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
                GameId = usergame.GameId,
                UserId = usergame.UserId,
                Description = game.Description,
                GenreName = game.GenreId != null ? games.GetGenre(game.GenreId).Name ?? String.Empty : String.Empty,
                ImageUrl = game.ImageUrl ?? String.Empty,
                IsComplete = usergame.IsComplete,
                MinAgeRequired = game.MinAgeRequired.ToString() ?? String.Empty,
                PlayerCount = playerCount ?? String.Empty,
                PublishDate = game.PublishDate,
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
                GameId = usergame.GameId,
                UserId = usergame.UserId,
                Description = game.Description,
                GenreName = game.GenreId != null ? games.GetGenre(game.GenreId).Name ?? String.Empty : String.Empty,
                ImageUrl = game.ImageUrl ?? String.Empty,
                IsComplete = usergame.IsComplete,
                MinAgeRequired = game.MinAgeRequired.ToString() ?? String.Empty,
                PlayerCount = playerCount ?? String.Empty,
                PublishDate = game.PublishDate,
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
                PublishDate = game.PublishDate,
                Publisher = game.Publisher,
                Title = game.Title
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
            existingGame.PublishDate = updatedGame.PublishDate;
            existingGame.Publisher = updatedGame.Publisher;
            existingGame.Title = updatedGame.Title;

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
            List<UserGameView> searchedGameViews = new List<UserGameView>();
            foreach (var game in gamePieces)
            {
                var gameCopy = GetGameDetails(game.GameId);
                searchedGameViews.Add(new UserGameView()
                {
                    GameId = game.GameId,
                    Title = gameCopy.Title,
                    Description = gameCopy.Description,
                    UserGameDescription = game.UserGameDescription,
                    ImageUrl = gameCopy.ImageUrl,
                    Publisher = gameCopy.Publisher,
                    PublishDate = gameCopy.PublishDate,
                    GenreName = gameCopy.GenreName,
                    PlayerCount = gameCopy.PlayerCount,
                    MinAgeRequired = gameCopy.MinAgeRequired,
                    State = game.State,
                    IsComplete = game.IsComplete,
                    Shipment = game.Shipment
                });
            }
            return searchedGameViews;
        }
    }
}

using ExchangeService.Controllers.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                State = newUserGameDto.State
            };
            userProfiles.AddGame(newUserGame);
            unitOfWork.CompleteWork();
            return newUserGame.GameId != 0;
        }

        public bool AddGame(GameDto game)
        {
            Game newGame = new Game()
            {
                Description = game.Description,
                GenreId = game.GenreId,
                ImageUrl = game.ImageUrl,
                MaxPlayerCount = game.MaxPlayerCount,
                MinPlayerCount = game.MinPlayerCount,
                PublishDate = game.PublishDate,
                Publisher = game.Publisher,
                Title = game.Title
            };
            games.AddGame(newGame);
            unitOfWork.CompleteWork();
            return newGame.GameId != 0;
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

            return new GameDto()
            {
                Description = game.Description,
                GameId = game.GameId,
                GenreId = game.GameId,
                ImageUrl = game.ImageUrl,
                MaxPlayerCount = game.MaxPlayerCount,
                MinPlayerCount = game.MinPlayerCount,
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
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.ImageUrl = updatedGame.ImageUrl;
            existingGame.MaxPlayerCount = updatedGame.MaxPlayerCount;
            existingGame.MinPlayerCount = updatedGame.MinPlayerCount;
            existingGame.PublishDate = updatedGame.PublishDate;
            existingGame.Publisher = updatedGame.Publisher;
            existingGame.Title = updatedGame.Title;

            unitOfWork.CompleteWork();
            return true;
        }

        public bool UpdateUserGame(UserGameDto updatedGame, int userId)
        {
            var existingGame = userProfiles.GetGame(updatedGame.GameId, userId);
            if (existingGame == null)
                return false;

            existingGame.State = updatedGame.State;
            existingGame.IsComplete = updatedGame.IsComplete;
            existingGame.Shipment = updatedGame.Shipment;

            unitOfWork.CompleteWork();
            return true;
        }

        public IEnumerable<UserSearchGameView> GetUsersSearchGames(int userId)
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
    }
}

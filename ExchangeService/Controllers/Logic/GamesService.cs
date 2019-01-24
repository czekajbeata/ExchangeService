using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ExchangeService.Controllers.Logic
{
    public class GamesService
    {
        private readonly IGames games;
        private readonly IUnitOfWork unitOfWork;

        public GamesService(IGames games, IUnitOfWork unitOfWork)
        {
            this.games = games;
            this.unitOfWork = unitOfWork;
        }
        
        public IEnumerable<GameView> GetAllGames(string query)
        {
            var gamesDetails = games.GetGames(query);
            List<GameView> gameDtos = new List<GameView>();
            foreach (var game in gamesDetails)
            {
                var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
                gameDtos.Add(new GameView()
                {
                    Description = game.Description,
                    GameId = game.GameId,
                    GenreName = games.GetGenre(game.GenreId).Name,
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
                GenreId = game.GenreId,
                ImageUrl = game.ImageUrl ?? null,
                MaxPlayerCount = games.GetPlayerCounts(game.PlayerCount).Item2,
                MinPlayerCount = games.GetPlayerCounts(game.PlayerCount).Item1,
                MinAgeRequired = Int32.Parse(game.MinAgeRequired),
                Publisher = game.Publisher ?? String.Empty,
                Title = game.Title,
                GameTimeInMin = Int32.Parse(game.GameTimeInMin)
            };
            games.AddGame(newGame);
            unitOfWork.CompleteWork();
            return newGame.GameId != 0;
        }

        public GameDto GetGameDetails(int id)
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

        public GameView GetGameView(int id)
        {
            var game = games.GetGame(id);
            if (game is null)
                return null;
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            return new GameView()
            {
                Description = game.Description,
                GameId = game.GameId,
                GenreName = games.GetGenre(game.GenreId).Name,
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
            existingGame.GenreId = updatedGame.GenreId;
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
    }
}

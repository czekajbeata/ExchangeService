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
        private readonly MappingService mappingService;

        public GamesService(IGames games, IUnitOfWork unitOfWork, MappingService mappingService)
        {
            this.games = games;
            this.unitOfWork = unitOfWork;
            this.mappingService = mappingService;
        }
        
        public IEnumerable<GameView> GetAllGames(string query)
        {
            var gamesDetails = games.GetGames(query);
            List<GameView> gameDtos = new List<GameView>();
            foreach (var game in gamesDetails)
            {
                var genre = games.GetGenre(game.GenreId).Name;
                gameDtos.Add(mappingService.GetGameViewFromGame(game, genre));
            }
            return gameDtos;
        }

        public bool AddGame(GameDto game)
        {
            Game newGame = mappingService.GetGameFromGameDto(game);
            games.AddGame(newGame);
            unitOfWork.CompleteWork();
            return newGame.GameId != 0;
        }

        public GameDto GetGameDetails(int id)
        {
            var game = games.GetGame(id);
            if (game is null)
                return null;
            return mappingService.GetGameDtoFromGame(game);
        }

        public GameView GetGameView(int id)
        {
            var game = games.GetGame(id);
            if (game is null)
                return null;
            var genre = games.GetGenre(game.GenreId).Name;
            return mappingService.GetGameViewFromGame(game, genre);
        }

        public bool UpdateGame(GameDto updatedGame)
        {
            var existingGame = games.GetGame(updatedGame.GameId);
            if (existingGame == null)
                return false;

            existingGame.Description = updatedGame.Description;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.ImageUrl = updatedGame.ImageUrl;
            existingGame.MaxPlayerCount = mappingService.GetPlayerCounts(updatedGame.PlayerCount).Item2;
            existingGame.MinPlayerCount = mappingService.GetPlayerCounts(updatedGame.PlayerCount).Item1;
            existingGame.MinAgeRequired = Int32.Parse(updatedGame.MinAgeRequired);
            existingGame.Publisher = updatedGame.Publisher;
            existingGame.Title = updatedGame.Title;
            existingGame.GameTimeInMin = Int32.Parse(updatedGame.GameTimeInMin);

            unitOfWork.CompleteWork();
            return true;
        }
    }
}

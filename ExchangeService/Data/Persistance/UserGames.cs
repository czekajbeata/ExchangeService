using ExchangeService.Core;
using ExchangeService.Core.Entities;
using ExchangeService.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeService.Data.Persistance
{
    public class UserGames : IUserGames
    {
        private readonly ApplicationDbContext context;

        public UserGames(ApplicationDbContext context)
        {
            this.context = context;
        }

        public UserGame AddGame(UserGame game)
        {
            var result = context.UserGames.Add(game);
            return result.Entity;
        }

        public bool DeleteUserGame(int userGameId)
        {
            var game = context.UserGames.FirstOrDefault(g => g.UserGameId == userGameId);
            if (game == null)
                return false;
            context.UserGames.Remove(game);
            return true;
        }

        public UserGame GetUserGame(int gameId, int userId)
        {
            return context.UserGames.SingleOrDefault(g => g.GameId == gameId && g.UserId == userId);
        }

        public UserGame GetUserGame(int userGameId)
        {
            return context.UserGames.SingleOrDefault(g => g.UserGameId == userGameId);
        }

        public IEnumerable<UserGame> GetUserGames(int userId)
        {
            return context.UserGames.Where(g => g.UserId == userId);
        }

        public IEnumerable<UserGame> GetUserGamesByGame(int gameId)
        {
            return context.UserGames.Where(g => g.GameId == gameId);
        }

        public IEnumerable<UserGame> GetAllUserGames()
        {
            return context.UserGames;
        }
    }
}

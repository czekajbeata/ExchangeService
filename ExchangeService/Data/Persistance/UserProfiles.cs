using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Data.Persistance
{
    public class UserProfiles : IUserProfiles
    {
        private readonly ApplicationDbContext context;

        public UserProfiles(ApplicationDbContext context)
        {
            this.context = context;
        }

        public UserGame AddGame(UserGame game)
        {
            var result = context.UserGames.Add(game);
            return result.Entity;
        }

        public UserSearchGame AddSearchGame(UserSearchGame game)
        {
            var result = context.UserSearchGames.Add(game);
            return result.Entity;
        }

        public UserGame GetGame(int gameId, int userId)
        {
            return context.UserGames.SingleOrDefault(g => g.GameId == gameId && g.UserId == userId);
        }

        public IEnumerable<UserSearchGame> GetUserSearchGames(int userId)
        {
            return context.UserSearchGames.Where(g => g.UserId == userId);
        }
    }
}

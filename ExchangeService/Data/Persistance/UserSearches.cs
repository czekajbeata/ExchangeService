using ExchangeService.Core;
using ExchangeService.Core.Entities;
using ExchangeService.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeService.Data.Persistance
{
    public class UserSearches : IUserSearches
    {
        private readonly ApplicationDbContext context;

        public UserSearches(ApplicationDbContext context)
        {
            this.context = context;
        }

        public UserSearchGame AddSearchGame(UserSearchGame game)
        {
            var result = context.UserSearchGames.Add(game);
            return result.Entity;
        }

        public bool DeleteUserSearch(int userSearchId)
        {
            var search = context.UserSearchGames.FirstOrDefault(g => g.UserSearchGameId == userSearchId);
            if (search == null)
                return false;
            context.UserSearchGames.Remove(search);
            return true;
        }

        public UserSearchGame GetUserSearch(int userSearchId)
        {
            return context.UserSearchGames.SingleOrDefault(u => u.UserSearchGameId == userSearchId);
        }

        public UserSearchGame GetUserSearch(int gameId, int userId)
        {
            return context.UserSearchGames.FirstOrDefault(u => u.UserId == userId && u.GameId == gameId);
        }

        public IEnumerable<UserSearchGame> GetUserSearchGames(int userId)
        {
            return context.UserSearchGames.Where(g => g.UserId == userId);
        }

        public IEnumerable<UserSearchGame> GetUserSearchesByGame(int gameId)
        {
            return context.UserSearchGames.Where(g => g.GameId == gameId);
        }

        public IEnumerable<UserSearchGame> GetAllUserSearches()
        {
            return context.UserSearchGames;
        }
    }
}

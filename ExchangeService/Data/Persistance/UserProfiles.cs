using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeService.Data.Persistance
{
    public class UserProfiles : IUserProfiles
    {
        private readonly ApplicationDbContext context;

        public UserProfiles(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Comment AddComment(Comment comment)
        {
            var result = context.Comments.Add(comment);
            return result.Entity;
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

        public IEnumerable<Comment> GetComments(int userId)
        {
            return context.Comments.Where(c => c.ReceivingUserId == userId);
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

        public IEnumerable<UserSearchGame> GetUserSearchGames(int userId)
        {
            return context.UserSearchGames.Where(g => g.UserId == userId);
        }

        public IEnumerable<UserGame> GetUserGamesByGame(int gameId)
        {
            return context.UserGames.Where(g => g.GameId == gameId);
        }

        public IEnumerable<UserSearchGame> GetUserSearchesByGame(int gameId)
        {
            return context.UserSearchGames.Where(g => g.GameId == gameId);
        }

        public User AddUserProfile(User newUser)
        {
            var result = context.UserProfiles.Add(newUser);
            return result.Entity;
        }

        public User GetUserProfile(int userId)
        {
            return context.UserProfiles.SingleOrDefault(u => u.UserId == userId);
        }

        public int GetNormalizedId(string innerId)
        {
            return context.UserProfiles.SingleOrDefault(u => u.InnerUserId == innerId).UserId;
        }

        public IEnumerable<Exchange> GetUserExchanges(int userId)
        {
            return context.Exchanges.Where(e => e.OfferingUserId  == userId || e.OtherUserId == userId);
        }

        public Exchange AddExchange(Exchange exchange)
        {
            var result = context.Exchanges.Add(exchange);
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

        public Exchange GetExchange(int exchangeId)
        {
            return context.Exchanges.SingleOrDefault(e => e.ExchangeId == exchangeId);
        }

        public Comment GetCommentByExchange(int exchangeId, int leavingUserId)
        {
            return context.Comments.SingleOrDefault(c => c.ConnectedExchangeId == exchangeId && c.LeavingUserId == leavingUserId);
        }
    }
}

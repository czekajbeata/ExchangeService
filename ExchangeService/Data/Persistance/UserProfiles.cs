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

        public IEnumerable<Comment> GetAllComments(int userId)
        {
            return context.Comments.Where(c => c.ReceivingUserId == userId);
        }

        public UserGame GetGame(int gameId, int userId)
        {
            return context.UserGames.SingleOrDefault(g => g.GameId == gameId && g.UserId == userId);
        }

        public IEnumerable<UserGame> GetUserGames(int userId)
        {
            return context.UserGames.Where(g => g.UserId == userId);
        }

        public double GetAvgMark(int userId)
        {
            var marks = context.Comments.Where(c => c.ReceivingUserId == userId).Select(o => o.Mark).ToArray();
            double sum = 0;
            foreach(var mark in marks)
            {
                sum += mark;
            }
            return sum / marks.Count();
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
    }
}

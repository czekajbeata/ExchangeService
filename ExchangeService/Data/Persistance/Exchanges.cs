using ExchangeService.Core;
using ExchangeService.Core.Entities;
using ExchangeService.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeService.Data.Persistance
{
    public class Exchanges : IExchanges
    {
        private readonly ApplicationDbContext context;

        public Exchanges(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Exchange AddExchange(Exchange exchange)
        {
            var result = context.Exchanges.Add(exchange);
            return result.Entity;
        }
        
        public Exchange GetExchange(int exchangeId)
        {
            return context.Exchanges.SingleOrDefault(e => e.ExchangeId == exchangeId);
        }

        public Comment GetCommentByExchange(int exchangeId, int leavingUserId)
        {
            return context.Comments.SingleOrDefault(c => c.ConnectedExchangeId == exchangeId && c.LeavingUserId == leavingUserId);
        }

        public IEnumerable<Exchange> GetUserExchanges(int userId)
        {
            return context.Exchanges.Where(e => e.OfferingUserId == userId || e.OtherUserId == userId);
        }

        public void RemoveExchangeGames(int userId, string usersGames)
        {
            int[] gameIds = usersGames.Split(',').Select(g => Int32.Parse(g)).ToArray();
            foreach (var gameId in gameIds)
            {
                var game = context.UserGames.FirstOrDefault(g => g.GameId == gameId && g.UserId == userId);
                if (game != null)
                    context.UserGames.Remove(game);
            }
        }

        public void DeclineWaitingExchanges(int userId, string usersGames, int exchangeId)
        {
            int[] gameIds = usersGames.Split(',').Select(g => Int32.Parse(g)).ToArray();
            var userExchanges = context.Exchanges.Where(e => e.OfferingUserId == userId && e.State == ExchangeState.Waiting && e.ExchangeId != exchangeId);
            foreach (var exchange in userExchanges)
            {
                var games = exchange.OfferingUsersGames.Contains(',') ? exchange.OfferingUsersGames.Split(',') : new string[] { exchange.OfferingUsersGames };
                games = games.Where(g => gameIds.Contains(Int32.Parse(g))).ToArray();
                if (games.Count() > 0)
                    exchange.State = ExchangeState.Declined;
            }
            userExchanges = context.Exchanges.Where(e => e.OtherUserId == userId && e.State == ExchangeState.Waiting && e.ExchangeId != exchangeId);
            foreach (var exchange in userExchanges)
            {
                var games = exchange.OtherUsersGames.Contains(',') ? exchange.OtherUsersGames.Split(',') : new string[] { exchange.OtherUsersGames };
                games = games.Where(g => gameIds.Contains(Int32.Parse(g))).ToArray();
                if (games.Count() > 0)
                    exchange.State = ExchangeState.Declined;
            }
        }

    }
}

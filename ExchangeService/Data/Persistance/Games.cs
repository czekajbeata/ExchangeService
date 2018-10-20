using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Data.Persistance
{
    public class Games : IGames
    {
        private readonly ApplicationDbContext context;

        public Games(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Game AddGame(Game game)
        {
            var result = context.Games.Add(game);
            return result.Entity;
        }

        public Game GetGame(int id)
        {
            return context.Games.Find(id);
        }

        public IEnumerable<Game> GetGames(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                return context.Games.Where(g => g.Title.Contains(query, StringComparison.CurrentCultureIgnoreCase));
            }
            return context.Games;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            return context.Genres;
        }
    }
}

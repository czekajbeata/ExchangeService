using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public Genre GetGenre(int? id)
        {
            if (id != null) return context.Genres.FirstOrDefault(g => g.GenreId == id);
            return null;
        }

        public Genre GetGenreByName(string name)
        {
            if (name != null) return context.Genres.FirstOrDefault(g => g.Name == name);
            return null;
        }

        public (int,int) GetPlayerCounts(string playerCount)
        {
            string pattern = @"(\d+)\-(\d+)";
            var match = Regex.Match(playerCount, pattern);
            int min = Int32.Parse(match.Groups[1].Value);
            int max = Int32.Parse(match.Groups[2].Value);
            return (min, max);
        }
    }
}

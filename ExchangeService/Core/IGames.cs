using ExchangeService.Core.Entities;
using System.Collections.Generic;

namespace ExchangeService.Core
{
    public interface IGames
    {
        Game AddGame(Game game);
        IEnumerable<Game> GetGames(string query);
        Game GetGame(int id);
        Genre GetGenre(int? id);
        IEnumerable<Genre> GetAllGenres();
        Genre GetGenreByName(string name);
        (int, int) GetPlayerCounts(string playerCount);
    }
}

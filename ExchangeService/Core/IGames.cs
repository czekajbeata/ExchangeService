using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Core
{
    public interface IGames
    {
        Game Add(Game game);
        IEnumerable<Game> GetGames(string query);
        Game Get(int id);
        IEnumerable<Genre> GetAllGenres();
    }
}

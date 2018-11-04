using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeService.Controllers.Logic
{
    public class DropDownService
    {
        private readonly IGames gamesDao;
        private readonly IUserProfiles userProfilesDao;

        public DropDownService(IGames gamesDao, IUserProfiles userProfilesDao)
        {
            this.gamesDao = gamesDao;
            this.userProfilesDao = userProfilesDao;
        }

        public IEnumerable<DropDownItem> GetGamesList(string query)
        {
            var games = gamesDao.GetGames(query);
            return games.Select(g => new DropDownItem { Id = g.GameId, Name = g.Title });
        }

        public IEnumerable<DropDownItem> GetGenres()
        {
            var genres = gamesDao.GetAllGenres();
            return genres.Select(g => new DropDownItem { Id = g.GenreId, Name = g.Name });
        }
    }
}

using ExchangeService.Core.Entities;
using System.Collections.Generic;

namespace ExchangeService.Core
{
    public interface IUserSearches
    {
        UserSearchGame AddSearchGame(UserSearchGame game);
        UserSearchGame GetUserSearch(int userSearchId);
        UserSearchGame GetUserSearch(int gameId, int userId);
        IEnumerable<UserSearchGame> GetUserSearchGames(int userId);
        IEnumerable<UserSearchGame> GetUserSearchesByGame(int gameId);
        IEnumerable<UserSearchGame> GetAllUserSearches();
        bool DeleteUserSearch(int userSearchId);
    }
}

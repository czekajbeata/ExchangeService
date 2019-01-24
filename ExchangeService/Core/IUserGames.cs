using ExchangeService.Core.Entities;
using System.Collections.Generic;

namespace ExchangeService.Core
{
    public interface IUserGames
    {
        UserGame AddGame(UserGame game);
        UserGame GetUserGame(int userGameId);
        UserGame GetUserGame(int gameId, int userId);
        IEnumerable<UserGame> GetUserGames(int userId);
        IEnumerable<UserGame> GetUserGamesByGame(int gameId);
        IEnumerable<UserGame> GetAllUserGames();
        bool DeleteUserGame(int userGameId);
    }
}

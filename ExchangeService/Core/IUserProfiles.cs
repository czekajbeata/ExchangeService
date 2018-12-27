using ExchangeService.Core.Entities;
using System.Collections.Generic;

namespace ExchangeService.Core
{
    public interface IUserProfiles
    {
        UserGame AddGame(UserGame game);
        UserGame GetGame(int gameId, int userId);
        IEnumerable<UserGame> GetUserGames(int userId);
        UserSearchGame AddSearchGame(UserSearchGame game);
        IEnumerable<UserSearchGame> GetUserSearchGames(int userId);
        Comment AddComment(Comment comment);
        IEnumerable<Comment> GetAllComments(int userId);
        double GetAvgMark(int userId);
        IEnumerable<UserGame> GetUserGamesByGame(int gameId);
        IEnumerable<UserSearchGame> GetUserSearchesByGame(int gameId);
        User AddUserProfile(User newUser);
        User GetUserProfile(int userId);
        int GetNormalizedId(string innerId);
        IEnumerable<Exchange> GetUserExchanges(int userId);
    }
}

using ExchangeService.Core.Entities;
using System.Collections.Generic;

namespace ExchangeService.Core
{
    public interface IUserProfiles
    {
        UserGame AddGame(UserGame game);
        UserGame GetUserGame(int gameId, int userId);
        UserGame GetUserGame(int userGameId);
        IEnumerable<UserGame> GetUserGames(int userId);
        UserSearchGame AddSearchGame(UserSearchGame game);
        IEnumerable<UserSearchGame> GetUserSearchGames(int userId);
        Comment AddComment(Comment comment);
        IEnumerable<Comment> GetComments(int userId);
        IEnumerable<UserGame> GetUserGamesByGame(int gameId);
        IEnumerable<UserSearchGame> GetUserSearchesByGame(int gameId);
        User AddUserProfile(User newUser);
        User GetUserProfile(int userId);
        int GetNormalizedId(string innerId);
        IEnumerable<Exchange> GetUserExchanges(int userId);
        Exchange GetExchange(int exchangeId);
        Exchange AddExchange(Exchange newExchange);
        bool DeleteUserGame(int userGameId);
        bool DeleteUserSearch(int userSearchId);
        UserSearchGame GetUserSearch(int userGameId);
        Comment GetCommentByExchange(int exchangeId, int receivingUserId);
        void RemoveExchangeGames(int otherUserId, string otherUsersGames);
    }
}

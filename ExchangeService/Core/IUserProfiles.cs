using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}

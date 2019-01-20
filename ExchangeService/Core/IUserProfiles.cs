using ExchangeService.Core.Entities;
using System.Collections.Generic;

namespace ExchangeService.Core
{
    public interface IUserProfiles
    {
        Comment AddComment(Comment comment);
        IEnumerable<Comment> GetComments(int userId);
        User AddUserProfile(User newUser);
        User GetUserProfile(int userId);
        User GetUserByInnerId(string innerId);
        IEnumerable<User> GetAllUserProfiles();
        int GetNormalizedId(string innerId);
    }
}

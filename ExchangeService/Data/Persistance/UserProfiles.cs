using ExchangeService.Core;
using ExchangeService.Core.Entities;
using ExchangeService.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeService.Data.Persistance
{
    public class UserProfiles : IUserProfiles
    {
        private readonly ApplicationDbContext context;

        public UserProfiles(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Comment AddComment(Comment comment)
        {
            var result = context.Comments.Add(comment);
            return result.Entity;
        }

        public IEnumerable<Comment> GetComments(int userId)
        {
            return context.Comments.Where(c => c.ReceivingUserId == userId);
        }

        public int GetNormalizedId(string innerId)
        {
            return context.UserProfiles.SingleOrDefault(u => u.InnerUserId == innerId).UserId;
        }

        public User AddUserProfile(User newUser)
        {
            var result = context.UserProfiles.Add(newUser);
            return result.Entity;
        }
        
        public User GetUserProfile(int userId)
        {
            return context.UserProfiles.FirstOrDefault(u => u.UserId == userId);
        }

        public User GetUserByInnerId(string innerId)
        {
            return context.UserProfiles.SingleOrDefault(u => u.InnerUserId == innerId);
        }

        public IEnumerable<User> GetAllUserProfiles()
        {
            return context.UserProfiles;
        }
    }
}

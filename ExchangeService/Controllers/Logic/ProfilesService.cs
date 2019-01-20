using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeService.Shared.Enums;

namespace ExchangeService.Controllers.Logic
{
    public class ProfilesService
    {
        private readonly IUserProfiles userProfiles;
        private readonly IUnitOfWork unitOfWork;
        private readonly IExchanges exchanges;
        private readonly IUserSearches userSearches;
        private readonly IUserGames userGames;
        private readonly IGames games;

        public ProfilesService(IUserProfiles userProfiles, IUnitOfWork unitOfWork, IExchanges exchanges, IUserSearches userSearches, IUserGames userGames, IGames games)
        {
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
            this.exchanges = exchanges;
            this.userSearches = userSearches;
            this.userGames = userGames;
            this.games = games;
        }
        
        public int ToNormalizedId(string innerId)
        {
            return userProfiles.GetNormalizedId(innerId);
        }

        public bool AddUserProfile(UserView user, string innerUserId)
        {
            User newUser = new User()
            {
                InnerUserId = innerUserId,
                Location = user.Location ?? String.Empty,
                Name = user.Name,
                Surname = user.Surname ?? String.Empty,
                ImageUrl = user.ImageUrl ?? String.Empty,
                PhoneNumber = user.PhoneNumber ?? "not given",
                ContactEmail = user.ContactEmail ?? "not given"

            };
            userProfiles.AddUserProfile(newUser);
            unitOfWork.CompleteWork();
            return newUser.UserId != 0;
        }

        public UserView GetUserProfile(int userId)
        {
            var user = userProfiles.GetUserProfile(userId);
            var userExchanges = exchanges.GetUserExchanges(userId);
            userExchanges = userExchanges.Where(e => e.State != ExchangeState.Declined).ToArray();
            var comments = userProfiles.GetComments(userId);
            var avgMark = comments.Select(c => c.Mark).Sum() / comments.Count();
            if (!(avgMark > 0)) avgMark = 0;
            else avgMark = Math.Round(avgMark, 2);
            return new UserView()
            {
                UserId = user.UserId,
                Location = user.Location,
                Name = user.Name,
                Surname = user.Surname,
                ImageUrl = user.ImageUrl,
                PhoneNumber = user.PhoneNumber,
                ContactEmail = user.ContactEmail,
                AvgMark = avgMark,
                ExchangesCount = userExchanges.Count(),
                ReviewsCount = comments.Count()
            };
        }

        public bool DoesProfileExist(string innerId)
        {
            return userProfiles.GetUserByInnerId(innerId) != null;
        }

        public bool UpdateUserProfile(UserView user)
        {
            var existingProfile = userProfiles.GetUserProfile(user.UserId);
            if (existingProfile == null)
                return false;
            existingProfile.ImageUrl = user.ImageUrl;
            existingProfile.Location = user.Location;
            existingProfile.Name = user.Name;
            existingProfile.Surname = user.Surname;
            existingProfile.PhoneNumber = String.IsNullOrEmpty(user.PhoneNumber) ? "not given" : user.PhoneNumber;
            existingProfile.ContactEmail = String.IsNullOrEmpty(user.ContactEmail) ? "not given" : user.ContactEmail;
            unitOfWork.CompleteWork();
            return true;
        }

        public IEnumerable<CommentDto> GetComments(int userId)
        {
            var userComments = userProfiles.GetComments(userId);
            List<CommentDto> commentDtos = new List<CommentDto>();
            foreach (var comment in userComments)
            {
                commentDtos.Add(new CommentDto()
                {
                    LeavingUserId = comment.LeavingUserId,
                    CommentDate = comment.CommentDate,
                    Mark = comment.Mark,
                    Text = comment.Text,
                    IsVisible = comment.IsVisible,
                    ConnectedExchangeId = comment.ConnectedExchangeId
                });
            }
            return commentDtos;
        }
        
        public IEnumerable<MatchAndUserView> GetMatches(int userId)
        {
            var allUserGames = userGames.GetAllUserGames();
            var allUserSearches = userSearches.GetAllUserSearches();
            var allGames = games.GetGames(" ");
            var myForExchangeIds = allUserGames.Where(g => g.UserId == userId).Select(g => g.GameId);
            var mySearchesIds = allUserSearches.Where(g => g.UserId == userId).Select(g => g.GameId);
            var matchedBySearches = allUserGames.Where(g => mySearchesIds.Contains(g.GameId) && g.UserId != userId).Select(g => g.UserId).Distinct();
            var matchedByForExchange = allUserSearches.Where(g => myForExchangeIds.Contains(g.GameId) && g.UserId != userId).Select(g => g.UserId).Distinct();
            var matchedUsers = matchedByForExchange.Where(u => matchedBySearches.Contains(u));
            List<MatchAndUserView> myMatches = new List<MatchAndUserView>();
            foreach (var user in matchedUsers)
            {
                var gamesTheyHaveIds = allUserGames.Where(g => g.UserId == user).Select(g => g.GameId);
                var gamesTheyWantIds = allUserSearches.Where(g => g.UserId == user).Select(g => g.GameId);

                var profile = userProfiles.GetUserProfile(user);
                var comments = userProfiles.GetComments(user);
                var avgMark = comments.Select(c => c.Mark).Sum() / comments.Count();
                if (!(avgMark > 0)) avgMark = 0;
                else avgMark = Math.Round(avgMark, 2);

                myMatches.Add(new MatchAndUserView()
                {
                    OtherUserId = user,
                    Name = profile.Name + " " + profile.Surname,
                    AvgMark = avgMark,
                    Location = profile.Location,
                    UserImageUrl = profile.ImageUrl,
                    GamesTheyHave = allGames.Where(g => gamesTheyHaveIds.Contains(g.GameId)).Select(g => g.Title).ToArray(),
                    GamesTheyWant = allGames.Where(g => gamesTheyWantIds.Contains(g.GameId)).Select(g => g.Title).ToArray()
                });
            }
            return myMatches;
        }

    }
}

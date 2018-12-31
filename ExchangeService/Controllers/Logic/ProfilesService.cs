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
        private readonly IGames games;

        public ProfilesService(IUserProfiles userProfiles, IUnitOfWork unitOfWork, IGames games)
        {
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
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
                Delivery = user.Delivery,
                Pickup = user.Pickup,
                PickUpLocation = user.PickUpLocation ?? String.Empty,
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
            var exchanges = userProfiles.GetUserExchanges(userId);
            var comments = userProfiles.GetComments(userId);
            var avgMark = comments.Select(c => c.Mark).Sum() / comments.Count();
            if (!(avgMark > 0)) avgMark = 0;
            return new UserView()
            {
                UserId = user.UserId,
                Delivery = user.Delivery,
                Pickup = user.Pickup,
                PickUpLocation = user.PickUpLocation,
                Name = user.Name,
                Surname = user.Surname,
                ImageUrl = user.ImageUrl,
                PhoneNumber = user.PhoneNumber,
                ContactEmail = user.ContactEmail,
                AvgMark = avgMark,
                ExchangesCount = exchanges.Count(),
                ReviewsCount = comments.Count()
            };
        }
    }
}

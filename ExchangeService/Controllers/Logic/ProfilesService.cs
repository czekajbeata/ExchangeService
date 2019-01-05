﻿using ExchangeService.Shared.Resources;
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
            var exchanges = userProfiles.GetUserExchanges(userId);
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
                ExchangesCount = exchanges.Count(),
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
    }
}

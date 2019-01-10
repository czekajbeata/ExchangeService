using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeService.Shared.Enums;

namespace ExchangeService.Controllers.Logic
{
    public class UserDataService
    {
        private readonly IUserProfiles userProfiles;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGames games;

        public UserDataService(IUserProfiles userProfiles, IUnitOfWork unitOfWork, IGames games)
        {
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
            this.games = games;
        }

        public bool AddComment(CommentDto comment)
        {
            var connectedExchange = GetShortenedExchange(comment.ConnectedExchangeId);
            int receivingUserId;
            if (comment.LeavingUserId == connectedExchange.FirstUserId)
                receivingUserId = connectedExchange.SecondUserId;
            else
                receivingUserId = connectedExchange.FirstUserId;

            Comment newComment = new Comment()
            {
                ReceivingUserId = receivingUserId,
                LeavingUserId = comment.LeavingUserId,
                CommentDate = DateTime.Now,
                Mark = comment.Mark,
                Text = comment.Text,
                ConnectedExchangeId = comment.ConnectedExchangeId,
                IsVisible = false
            };

            if (connectedExchange.State == ExchangeState.Finalized)
            {
                Comment secondComment = userProfiles.GetCommentByExchange(connectedExchange.ExchangeId, receivingUserId);
                if(secondComment != null)
                    secondComment.IsVisible = true;
                newComment.IsVisible = true;
            }
            userProfiles.AddComment(newComment);
            unitOfWork.CompleteWork();
            return newComment.CommentId != 0;
        }

        public bool AddExchange(ExchangeDto exchange, int normalizedId)
        {
            string offering = exchange.MyGamesIds.Count() > 1 ? string.Join(",", exchange.MyGamesIds) : exchange.MyGamesIds[0].ToString();
            string others = exchange.OtherUserGamesIds.Count() > 1 ? string.Join(",", exchange.OtherUserGamesIds) : exchange.OtherUserGamesIds[0].ToString();

            Exchange newExchange = new Exchange()
            {
                OfferingUserId = normalizedId,
                OtherUserId = exchange.OtherUserId,
                OfferingUsersGames = offering,
                OtherUsersGames = others,
                Shipment = exchange.Shipment,
                State = exchange.State,
                OfferingUserContactInfo = exchange.MyContactInfo ?? string.Empty,
                OtherUserContactInfo = exchange.OtherUserContactInfo ?? string.Empty,
                OfferingUserFinalizeTime = exchange.MyFinalizeTime,
                OtherUserFinalizeTime = exchange.OtherUserFinalizeTime
            };
            userProfiles.AddExchange(newExchange);
            unitOfWork.CompleteWork();
            return newExchange.ExchangeId != 0;
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

        public ShortenedExchangeView GetShortenedExchange(int exchangeId)
        {
            var exchange = userProfiles.GetExchange(exchangeId);
            return new ShortenedExchangeView()
            {
                ExchangeId = exchange.ExchangeId,
                State = exchange.State,
                FirstUserId = exchange.OfferingUserId,
                SecondUserId = exchange.OtherUserId
            };
        }

        public bool AbandonExchange(int exchangeId)
        {
            var existingExchange = userProfiles.GetExchange(exchangeId);
            if (existingExchange == null)
                return false;
            existingExchange.State = ExchangeState.Declined;
            unitOfWork.CompleteWork();
            return true;
        }

        public bool AcceptExchange(ExchangeDto exchange)
        {
            var existingExchange = userProfiles.GetExchange(exchange.ExchangeId);
            if (existingExchange == null)
                return false;
            existingExchange.State = ExchangeState.InProgress;
            existingExchange.OtherUserContactInfo = exchange.MyContactInfo;
            userProfiles.RemoveExchangeGames(existingExchange.OfferingUserId, existingExchange.OfferingUsersGames);
            userProfiles.RemoveExchangeGames(existingExchange.OtherUserId, existingExchange.OtherUsersGames);
            unitOfWork.CompleteWork();
            return true;
        }

        public bool FinalizeExchange(ExchangeDto exchange, int myUserId)
        {
            var existingExchange = userProfiles.GetExchange(exchange.ExchangeId);
            if (existingExchange == null)
                return false;

            if (existingExchange.OfferingUserId == myUserId)
                existingExchange.OfferingUserFinalizeTime = DateTime.Now;
            else          
                existingExchange.OtherUserFinalizeTime = DateTime.Now;

            if (existingExchange.OtherUserFinalizeTime.Year >= 2018
                && existingExchange.OfferingUserFinalizeTime.Year >= 2018)
                existingExchange.State = ExchangeState.Finalized;

            unitOfWork.CompleteWork();
            return true;
        }

        public ExchangeDto GetExchange(int exchangeId, int userId)
        {
            var exchange = userProfiles.GetExchange(exchangeId);
            var newExchangeDto = new ExchangeDto()
            {
                ExchangeId = exchange.ExchangeId,
                Shipment = exchange.Shipment,
                State = exchange.State
            };
            if (exchange.OfferingUserId == userId)
            {
                newExchangeDto.OtherUserId = exchange.OtherUserId;
                newExchangeDto.MyGamesIds = exchange.OfferingUsersGames.Split(',').ToArray();
                newExchangeDto.OtherUserGamesIds = exchange.OtherUsersGames.Split(',').ToArray();
                newExchangeDto.MyFinalizeTime = exchange.OfferingUserFinalizeTime;
                newExchangeDto.OtherUserFinalizeTime = exchange.OtherUserFinalizeTime;
                newExchangeDto.MyContactInfo = exchange.OfferingUserContactInfo ?? string.Empty;
                newExchangeDto.OtherUserContactInfo = exchange.OtherUserContactInfo ?? string.Empty;
            }
            else
            {
                newExchangeDto.OtherUserId = exchange.OfferingUserId;
                newExchangeDto.MyGamesIds = exchange.OtherUsersGames.Split(',').ToArray();
                newExchangeDto.OtherUserGamesIds = exchange.OfferingUsersGames.Split(',').ToArray();
                newExchangeDto.MyFinalizeTime = exchange.OtherUserFinalizeTime;
                newExchangeDto.OtherUserFinalizeTime = exchange.OfferingUserFinalizeTime;
                newExchangeDto.OtherUserContactInfo = exchange.OfferingUserContactInfo ?? string.Empty;
                newExchangeDto.MyContactInfo = exchange.OtherUserContactInfo ?? string.Empty;
            }
            return newExchangeDto;
        }

        public IEnumerable<ExchangeDto> GetUserExchanges(int userId)
        {
            var myExchanges = userProfiles.GetUserExchanges(userId);
            List<ExchangeDto> exchanges = new List<ExchangeDto>();
            foreach (var exchange in myExchanges)
            {
                var newExchangeDto = new ExchangeDto()
                {
                    ExchangeId = exchange.ExchangeId,
                    Shipment = exchange.Shipment,
                    OtherUserContactInfo = exchange.OtherUserContactInfo,
                    State = exchange.State
                };
                if (exchange.OfferingUserId == userId)
                {
                    newExchangeDto.OtherUserId = exchange.OtherUserId;
                    newExchangeDto.MyGamesIds = exchange.OfferingUsersGames.Split(',').ToArray();
                    newExchangeDto.OtherUserGamesIds = exchange.OtherUsersGames.Split(',').ToArray();
                    newExchangeDto.MyFinalizeTime = exchange.OfferingUserFinalizeTime;
                    newExchangeDto.OtherUserFinalizeTime = exchange.OtherUserFinalizeTime;
                    newExchangeDto.MyContactInfo = exchange.OfferingUserContactInfo ?? string.Empty;
                    newExchangeDto.OtherUserContactInfo = exchange.OtherUserContactInfo ?? string.Empty;
                }
                else
                {
                    newExchangeDto.OtherUserId = exchange.OfferingUserId;
                    newExchangeDto.MyGamesIds = exchange.OtherUsersGames.Split(',').ToArray();
                    newExchangeDto.OtherUserGamesIds = exchange.OfferingUsersGames.Split(',').ToArray();
                    newExchangeDto.MyFinalizeTime = exchange.OtherUserFinalizeTime;
                    newExchangeDto.OtherUserFinalizeTime = exchange.OfferingUserFinalizeTime;
                    newExchangeDto.OtherUserContactInfo = exchange.OfferingUserContactInfo ?? string.Empty;
                    newExchangeDto.MyContactInfo = exchange.OtherUserContactInfo ?? string.Empty;
                }
                exchanges.Add(newExchangeDto);
            }
            return exchanges;
        }

        public IEnumerable<ExchangeView> GetMyExchanges(int userId)
        {
            var myExchanges = userProfiles.GetUserExchanges(userId);
            List<ExchangeView> exchanges = new List<ExchangeView>();
            foreach (var exchange in myExchanges)
            {
                var newExchangeView = new ExchangeView()
                {
                    ExchangeId = exchange.ExchangeId,
                    Shipment = exchange.Shipment,
                    State = exchange.State
                };
                if (exchange.OfferingUserId == userId)
                {
                    newExchangeView.AmIOffering = true;
                    var user = userProfiles.GetUserProfile(exchange.OtherUserId);
                    newExchangeView.OtherUserName = user.Name + " " + user.Surname;
                    newExchangeView.UserImage = user.ImageUrl;
                    newExchangeView.OtherUserId = exchange.OtherUserId;
                    var myGames = exchange.OfferingUsersGames.Split(',').ToArray();
                    var otherUserGames = exchange.OtherUsersGames.Split(',').ToArray();
                    for (int i = 0; i < myGames.Count(); i++)
                    {
                        myGames[i] = games.GetGame(Int32.Parse(myGames[i])).Title;
                    }
                    for (int i = 0; i < otherUserGames.Count(); i++)
                    {
                        otherUserGames[i] = games.GetGame(Int32.Parse(otherUserGames[i])).Title;
                    }
                    newExchangeView.MyGames = myGames;
                    newExchangeView.OtherUserGames = otherUserGames;
                    newExchangeView.MyFinalizeTime = exchange.OfferingUserFinalizeTime;
                    newExchangeView.OtherUserFinalizeTime = exchange.OtherUserFinalizeTime;
                }
                else
                {
                    newExchangeView.AmIOffering = false;
                    var user = userProfiles.GetUserProfile(exchange.OfferingUserId);
                    newExchangeView.OtherUserName = user.Name + " " + user.Surname;
                    newExchangeView.UserImage = user.ImageUrl;
                    newExchangeView.OtherUserId = exchange.OfferingUserId;
                    var myGames = exchange.OtherUsersGames.Split(',').ToArray();
                    var otherUserGames = exchange.OfferingUsersGames.Split(',').ToArray();
                    for (int i = 0; i < myGames.Count(); i++)
                    {
                        myGames[i] = games.GetGame(Int32.Parse(myGames[i])).Title;
                    }
                    for (int i = 0; i < otherUserGames.Count(); i++)
                    {
                        otherUserGames[i] = games.GetGame(Int32.Parse(otherUserGames[i])).Title;
                    }
                    newExchangeView.MyGames = myGames;
                    newExchangeView.OtherUserGames = otherUserGames;
                    newExchangeView.MyFinalizeTime = exchange.OtherUserFinalizeTime;
                    newExchangeView.OtherUserFinalizeTime = exchange.OfferingUserFinalizeTime;
                }
                if (newExchangeView.MyFinalizeTime.Year >= 2018)
                {
                    newExchangeView.HaveIFinalized = true;
                }
                exchanges.Add(newExchangeView);
            }
            return exchanges;
        }

        public IEnumerable<MatchAndUserView> GetMatches(int userId)
        {
            var allUserGames = userProfiles.GetAllUserGames();
            var allUserSearches = userProfiles.GetAllUserSearches();
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

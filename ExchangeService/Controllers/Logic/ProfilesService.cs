using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
        
        public bool AddComment(CommentDto comment, int receivingUserId)
        {
            Comment newComment = new Comment()
            {
                ReceivingUserId = receivingUserId,
                LeavingUserId = comment.LeavingUserId,
                CommentDate = DateTime.Today,
                Mark = comment.Mark,
                Text = comment.Text
            };
            userProfiles.AddComment(newComment);
            unitOfWork.CompleteWork();
            return newComment.CommentId != 0;
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
                    Text = comment.Text
                });
            }
            return commentDtos;
        }

        public IEnumerable<MatchView> GetMatches(int userId)
        {
            var usersSearchedGamesIds = userProfiles.GetUserSearchGames(userId).Select(g => g.GameId);
            var usersToExchangeGamesIds = userProfiles.GetUserGames(userId).Select(g => g.GameId);
            List<MatchView> userMatches = new List<MatchView>();

            // --- filling games they have that you want
            foreach(var searchedGame in usersSearchedGamesIds)
            {
                var otherUsersGames = userProfiles.GetUserGamesByGame(searchedGame);          
                foreach(var otherUsersGame in otherUsersGames)
                {
                    string gameTitle = games.GetGame(otherUsersGame.GameId).Title;
                    var existingMatch = userMatches.FirstOrDefault(m => m.OtherUserId == otherUsersGame.UserId);

                    if(existingMatch != null)
                    {
                        existingMatch.GamesTheyHave.Add(gameTitle);
                    }
                    else
                    {
                        userMatches.Add(new MatchView()
                        {
                            OtherUserId = otherUsersGame.UserId,
                            GamesTheyHave = new List<string>() { gameTitle },
                            GamesTheyWant = new List<string>() { }
                        });
                    }                    
                }
            }

            // --- filling games they want that you have
            foreach (var toExchange in usersToExchangeGamesIds)
            {
                var otherUsersGames = userProfiles.GetUserSearchesByGame(toExchange);
                foreach (var otherUsersGame in otherUsersGames)
                {
                    string gameTitle = games.GetGame(otherUsersGame.GameId).Title;
                    var existingMatch = userMatches.FirstOrDefault(m => m.OtherUserId == otherUsersGame.UserId);

                    if (existingMatch != null)
                    {
                        existingMatch.GamesTheyWant.Add(gameTitle);
                    }
                    else
                    {
                        userMatches.Add(new MatchView()
                        {
                            OtherUserId = otherUsersGame.UserId,
                            GamesTheyHave = new List<string>() { },
                            GamesTheyWant = new List<string>() { gameTitle }
                        });
                    }
                }
            }

            return userMatches;
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
                    Pickup = exchange.Pickup,
                    PickUpLocation = exchange.PickUpLocation,
                    Delivery = exchange.Delivery,
                    State = exchange.State
                };
                if (exchange.OfferingUserId == userId)
                {
                    newExchangeDto.OtherUserId = exchange.OtherUserId;
                    newExchangeDto.MyGamesIds = exchange.FirstUsersGames.Split(',').Select(g => Int32.Parse(g)).ToArray();
                    newExchangeDto.OtherUserGamesIds = exchange.OtherUsersGames.Split(',').Select(g => Int32.Parse(g)).ToArray();
                }
                else
                {
                    newExchangeDto.OtherUserId = exchange.OfferingUserId;
                    newExchangeDto.MyGamesIds = exchange.OtherUsersGames.Split(',').Select(g => Int32.Parse(g)).ToArray();
                    newExchangeDto.OtherUserGamesIds = exchange.FirstUsersGames.Split(',').Select(g => Int32.Parse(g)).ToArray();
                }
                exchanges.Add(newExchangeDto);
            }
            return exchanges;
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
                PickUpLocation = user.PickUpLocation,
                Name = user.Name,
                Surname = user.Surname,
                ImageUrl = user.ImageUrl,
                PhoneNumber = user.PhoneNumber != null ? user.PhoneNumber : "not given",
                ContactEmail = user.ContactEmail != null ? user.ContactEmail : "not given"

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

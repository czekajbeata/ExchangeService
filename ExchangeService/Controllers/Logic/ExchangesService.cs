using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeService.Shared.Enums;

namespace ExchangeService.Controllers.Logic
{
    public class ExchangesService
    {
        private readonly IUserProfiles userProfiles;
        private readonly IExchanges exchanges;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGames games;

        public ExchangesService(IUserProfiles userProfiles, IExchanges exchanges, IUnitOfWork unitOfWork)
        {
            this.userProfiles = userProfiles;
            this.exchanges = exchanges;
            this.unitOfWork = unitOfWork;
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
                Comment secondComment = exchanges.GetCommentByExchange(connectedExchange.ExchangeId, receivingUserId);
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
            exchanges.AddExchange(newExchange);
            unitOfWork.CompleteWork();
            return newExchange.ExchangeId != 0;
        }
        
        public bool AbandonExchange(int exchangeId)
        {
            var existingExchange = exchanges.GetExchange(exchangeId);
            if (existingExchange == null)
                return false;
            existingExchange.State = ExchangeState.Declined;
            unitOfWork.CompleteWork();
            return true;
        }

        public bool AcceptExchange(ExchangeDto exchange)
        {
            var existingExchange = exchanges.GetExchange(exchange.ExchangeId);
            if (existingExchange == null)
                return false;
            existingExchange.State = ExchangeState.InProgress;
            existingExchange.OtherUserContactInfo = exchange.MyContactInfo;
            exchanges.RemoveExchangeGames(existingExchange.OfferingUserId, existingExchange.OfferingUsersGames);
            exchanges.RemoveExchangeGames(existingExchange.OtherUserId, existingExchange.OtherUsersGames);
            exchanges.DeclineWaitingExchanges(existingExchange.OfferingUserId, existingExchange.OfferingUsersGames, existingExchange.ExchangeId);
            exchanges.DeclineWaitingExchanges(existingExchange.OtherUserId, existingExchange.OtherUsersGames, existingExchange.ExchangeId);
            unitOfWork.CompleteWork();
            return true;
        }

        public bool FinalizeExchange(ExchangeDto exchange, int myUserId)
        {
            var existingExchange = exchanges.GetExchange(exchange.ExchangeId);
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

        public ShortenedExchangeView GetShortenedExchange(int exchangeId)
        {
            var exchange = exchanges.GetExchange(exchangeId);
            return new ShortenedExchangeView()
            {
                ExchangeId = exchange.ExchangeId,
                State = exchange.State,
                FirstUserId = exchange.OfferingUserId,
                SecondUserId = exchange.OtherUserId
            };
        }

        public ExchangeDto GetExchange(int exchangeId, int userId)
        {
            var exchange = exchanges.GetExchange(exchangeId);
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
            var myExchanges = exchanges.GetUserExchanges(userId);
            List<ExchangeDto> listExchanges = new List<ExchangeDto>();
            foreach (var exchange in myExchanges)
            {
                var newExchangeDto = GetExchange(exchange.ExchangeId, userId);
                listExchanges.Add(newExchangeDto);
            }
            return listExchanges;
        }

        public IEnumerable<ExchangeView> GetMyExchanges(int userId)
        {
            var myExchanges = exchanges.GetUserExchanges(userId);
            List<ExchangeView> exchangeViews = new List<ExchangeView>();
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
                exchangeViews.Add(newExchangeView);
            }
            return exchangeViews;
        }
    }
}

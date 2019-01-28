using ExchangeService.Shared.Resources;
using ExchangeService.Core;
using System.Collections.Generic;
using System.Linq;
using ExchangeService.Core.Entities;
using System;
using System.Text.RegularExpressions;

namespace ExchangeService.Controllers.Logic
{
    public class MappingService
    {
        public MappingService()
        {
        }

        public Comment GetCommentFromCommentDto(CommentDto comment, int userId)
        {
            return new Comment()
            {
                ReceivingUserId = userId,
                LeavingUserId = comment.LeavingUserId,
                CommentDate = DateTime.Now,
                Mark = comment.Mark,
                Text = comment.Text,
                ConnectedExchangeId = comment.ConnectedExchangeId,
                IsVisible = false
            };
        }

        public CommentDto GetCommentDtoFromComment(Comment comment)
        {
            return new CommentDto()
            {
                LeavingUserId = comment.LeavingUserId,
                CommentDate = comment.CommentDate,
                Mark = comment.Mark,
                Text = comment.Text,
                IsVisible = comment.IsVisible,
                ConnectedExchangeId = comment.ConnectedExchangeId
            };
        }

        public UserView GetUserViewFromUser(User user, IEnumerable<Comment> comments, int exchangesCount)
        {
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
                ExchangesCount = exchangesCount,
                ReviewsCount = comments.Count()
            };
        }

        public User GetUserFromUserView(UserView user, string userId)
        {
            return new User()
            {
                InnerUserId = userId,
                Location = user.Location ?? String.Empty,
                Name = user.Name,
                Surname = user.Surname ?? String.Empty,
                ImageUrl = String.IsNullOrEmpty(user.ImageUrl) ? "https://upload.wikimedia.org/wikipedia/commons/8/89/Portrait_Placeholder.png" : user.ImageUrl,
                PhoneNumber = user.PhoneNumber ?? "not given",
                ContactEmail = user.ContactEmail ?? "not given"
            };
        }

        public Exchange GetExchangeFromExchangeDto(ExchangeDto exchange, int userId)
        {
            string offering = exchange.MyGamesIds.Count() > 1 ? string.Join(",", exchange.MyGamesIds) : exchange.MyGamesIds[0].ToString();
            string others = exchange.OtherUserGamesIds.Count() > 1 ? string.Join(",", exchange.OtherUserGamesIds) : exchange.OtherUserGamesIds[0].ToString();

            return new Exchange()
            {
                OfferingUserId = userId,
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
        }

        public ShortenedExchangeView GetShortenedExchangeView(Exchange exchange)
        {
            return new ShortenedExchangeView()
            {
                ExchangeId = exchange.ExchangeId,
                State = exchange.State,
                FirstUserId = exchange.OfferingUserId,
                SecondUserId = exchange.OtherUserId
            };
        }

        public ExchangeDto GetExchangeDtoFromExchange(Exchange exchange, int userId)
        {
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

        public (int, int) GetPlayerCounts(string playerCount)
        {
            string pattern = @"(\d+)\-(\d+)";
            var match = Regex.Match(playerCount, pattern);
            int min = Int32.Parse(match.Groups[1].Value);
            int max = Int32.Parse(match.Groups[2].Value);
            return (min, max);
        }

        public GameView GetGameViewFromGame(Game game, string genre)
        {
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            return new GameView()
            {
                Description = game.Description,
                GameId = game.GameId,
                GenreName = genre,
                ImageUrl = game.ImageUrl,
                PlayerCount = playerCount,
                MinAgeRequired = game.MinAgeRequired.ToString(),
                Publisher = game.Publisher,
                Title = game.Title,
                GameTimeInMin = game.GameTimeInMin.ToString()
            };
        }

        public Game GetGameFromGameDto(GameDto game)
        {
            return new Game()
            {
                Description = game.Description ?? String.Empty,
                GenreId = game.GenreId,
                ImageUrl = game.ImageUrl ?? null,
                MaxPlayerCount = GetPlayerCounts(game.PlayerCount).Item2,
                MinPlayerCount = GetPlayerCounts(game.PlayerCount).Item1,
                MinAgeRequired = Int32.Parse(game.MinAgeRequired),
                Publisher = game.Publisher ?? String.Empty,
                Title = game.Title,
                GameTimeInMin = Int32.Parse(game.GameTimeInMin)
            };
        }

        public GameDto GetGameDtoFromGame(Game game)
        {
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            return new GameDto()
            {
                Description = game.Description,
                GameId = game.GameId,
                GenreId = game.GenreId,
                ImageUrl = game.ImageUrl,
                PlayerCount = playerCount,
                MinAgeRequired = game.MinAgeRequired.ToString(),
                Publisher = game.Publisher,
                Title = game.Title,
                GameTimeInMin = game.GameTimeInMin.ToString()
            };
        }

        public UserGame GetUserGameFromDto(UserGameDto userGame, int userId)
        {
            string images = userGame.UserImages.Count() > 1
              ? string.Join(",", userGame.UserImages)
              : userGame.UserImages.Count() == 0
                  ? string.Empty
                  : userGame.UserImages[0];
            return new UserGame()
            {
                UserId = userId,
                GameId = userGame.GameId,
                IsComplete = userGame.IsComplete,
                Shipment = userGame.Shipment,
                State = userGame.State,
                UserGameDescription = userGame.UserGameDescription,
                UserGameImages = images
            };
        }

        public UserGameDto GetUserGameDtoFromUserGame(UserGame userGame)
        {
            string[] images = string.IsNullOrEmpty(userGame.UserGameImages)
           ? new string[] { }
           : userGame.UserGameImages.Contains(',')
               ? userGame.UserGameImages.Split(',')
               : new string[] { userGame.UserGameImages };
            return new UserGameDto()
            {
                GameId = userGame.GameId,
                UserId = userGame.UserId,
                IsComplete = userGame.IsComplete,
                Shipment = userGame.Shipment,
                State = userGame.State,
                UserGameDescription = userGame.UserGameDescription,
                UserImages = images
            };
        }

        public UserGameView GetUserGameView(Game game, string genre, UserGame userGame)
        {
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            string[] images = string.IsNullOrEmpty(userGame.UserGameImages)
            ? new string[] { }
            : userGame.UserGameImages.Contains(',')
                ? userGame.UserGameImages.Split(',')
                : new string[] { userGame.UserGameImages };
            return new UserGameView()
            {
                UserGameId = userGame.UserGameId,
                GameId = userGame.GameId,
                UserId = userGame.UserId,
                GameTimeInMin = game.GameTimeInMin.ToString(),
                Description = game.Description,
                GenreName = genre,
                ImageUrl = game.ImageUrl,
                IsComplete = userGame.IsComplete,
                MinAgeRequired = game.MinAgeRequired.ToString(),
                PlayerCount = playerCount,
                Publisher = game.Publisher ?? String.Empty,
                Shipment = userGame.Shipment,
                State = userGame.State,
                Title = game.Title,
                UserGameDescription = userGame.UserGameDescription,
                UserImages = images
            };
        }

        public GameAndUserView GetGameAndUserView(Game game, UserGame userGame, User user, IEnumerable<Comment> comments)
        {
            var avgMark = comments.Select(c => c.Mark).Sum() / comments.Count();
            if (!(avgMark > 0)) avgMark = 0;
            else avgMark = Math.Round(avgMark, 2);
            var playerCount = game.MinPlayerCount + "-" + game.MaxPlayerCount;
            return new GameAndUserView()
            {
                UserGameId = userGame.UserGameId,
                GameId = userGame.GameId,
                UserId = userGame.UserId,
                Title = game.Title,
                PlayerCount = playerCount,
                MinAgeRequired = game.MinAgeRequired.ToString(),
                GameTimeInMin = game.GameTimeInMin.ToString(),
                State = userGame.State,
                IsComplete = userGame.IsComplete,
                Shipment = userGame.Shipment,
                ImageUrl = game.ImageUrl,
                Name = user.Name,
                Surname = user.Surname,
                Location = user.Location,
                UserImageUrl = user.ImageUrl,
                AvgMark = avgMark
            };
        }

        public UserSearchGameView GetUserSearchGameView(Game game, UserSearchGame userSearchGame)
        {
            return new UserSearchGameView()
            {
                UserSearchId = userSearchGame.UserSearchGameId,
                UserId = userSearchGame.UserId,
                GameId = userSearchGame.GameId,
                Title = game.Title,
                ImageUrl = game.ImageUrl
            };
        }

        public UserSearchGame GetUserSearchGameFromDto(UserSearchGameDto userSearchGame, int userId)
        {
            return new UserSearchGame()
            {
                UserId = userId,
                GameId = userSearchGame.GameId
            };
        }
    }
}

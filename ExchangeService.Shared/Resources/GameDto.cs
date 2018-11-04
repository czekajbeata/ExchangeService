using System;

namespace ExchangeService.Shared.Resources
{
    public class GameDto
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? GenreId { get; set; }
        public int? MinPlayerCount { get; set; }
        public int? MaxPlayerCount { get; set; }
        public int? MinAgeRequired { get; set; }
    }
}

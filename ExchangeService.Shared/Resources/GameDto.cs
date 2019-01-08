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
        public int GenreId { get; set; }
        public string PlayerCount { get; set; }
        public string MinAgeRequired { get; set; }
        public string GameTimeInMin { get; set; }
    }
}

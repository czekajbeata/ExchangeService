using ExchangeService.Shared.Enums;
using System;

namespace ExchangeService.Shared.Resources
{
    public class GameAndUserView
    {
        public int UserGameId { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string PlayerCount { get; set; }
        public string MinAgeRequired { get; set; }
        public GameState State { get; set; }
        public bool IsComplete { get; set; }
        public Shipment Shipment { get; set; }
        public string UserImageUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Location { get; set; }
        public double AvgMark { get; set; }
    }
}

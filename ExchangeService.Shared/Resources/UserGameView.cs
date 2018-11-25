using ExchangeService.Shared.Enums;
using System;

namespace ExchangeService.Shared.Resources
{
    public class UserGameView
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserGameDescription { get; set; }
        public string ImageUrl { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public string GenreName { get; set; }
        public string PlayerCount { get; set; }
        public int? MinAgeRequired { get; set; }
        public State State { get; set; }
        public bool IsComplete { get; set; }
        public Shipment Shipment { get; set; }
    }
}

using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Controllers.Resources
{
    public class UserGameView
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishDate { get; set; }
        public string GenreName { get; set; }
        public int? MinPlayerCount { get; set; }
        public int? MaxPlayerCount { get; set; }
        public State State { get; set; }
        public bool IsComplete { get; set; }
        public Shipment Shipment { get; set; }
    }
}

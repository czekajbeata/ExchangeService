﻿using ExchangeService.Shared.Enums;

namespace ExchangeService.Core.Entities
{
    public class UserGame
    {
        public int UserGameId { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string UserGameDescription { get; set; }
        public State State { get; set; }
        public bool IsComplete { get; set; }
        public Shipment Shipment { get; set; }
    }
}

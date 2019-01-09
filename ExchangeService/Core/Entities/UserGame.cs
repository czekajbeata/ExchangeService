using ExchangeService.Shared.Enums;

namespace ExchangeService.Core.Entities
{
    public class UserGame
    {
        public int UserGameId { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string UserGameDescription { get; set; }
        public string UserGameImages { get; set; }
        public GameState State { get; set; }
        public bool IsComplete { get; set; }
        public Shipment Shipment { get; set; }
    }
}

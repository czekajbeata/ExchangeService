using ExchangeService.Shared.Enums;

namespace ExchangeService.Shared.Resources
{
    public class UserGameDto
    {
        public int GameId { get; set; }
        public GameState State { get; set; }
        public bool IsComplete { get; set; }
        public Shipment Shipment { get; set; }
        public string UserGameDescription { get; set; }
        public string[] UserImages { get; set; }
    }
}

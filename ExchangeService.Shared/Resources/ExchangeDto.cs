using ExchangeService.Shared.Enums;
using System;

namespace ExchangeService.Shared.Resources
{
    public class ExchangeDto
    {
        public int ExchangeId { get; set; }
        public int OtherUserId { get; set; }
        public int[] MyGamesIds { get; set; }
        public int[] OtherUserGamesIds { get; set; }
        public bool Pickup { get; set; }
        public string OfferingUserContactInfo { get; set; }
        public string OtherUserContactInfo { get; set; }
        public bool Delivery { get; set; }
        public ExchangeState State { get; set; }

    }
}

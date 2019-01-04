using ExchangeService.Shared.Enums;
using System;

namespace ExchangeService.Shared.Resources
{
    public class ExchangeDto
    {
        public int ExchangeId { get; set; }
        public int OtherUserId { get; set; }
        public string[] MyGamesIds { get; set; }
        public string[] OtherUserGamesIds { get; set; }
        public Shipment Shipment { get; set; }
        public string OfferingUserContactInfo { get; set; }
        public string OtherUserContactInfo { get; set; }
        public ExchangeState State { get; set; }
        public DateTime MyFinalizeTime { get; set; }
        public DateTime OtherUserFinalizeTime { get; set; }

    }
}

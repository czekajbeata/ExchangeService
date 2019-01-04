using ExchangeService.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExchangeService.Core.Entities
{
    public class Exchange
    {
        public int ExchangeId { get; set; }
        [Required]
        public int OfferingUserId { get; set; }
        [Required]
        public int OtherUserId { get; set; }
        [Required]
        public string OfferingUsersGames { get; set; }
        [Required]
        public string OtherUsersGames { get; set; }
        [Required]
        public Shipment Shipment { get; set; }
        [Required]
        public string OfferingUserContactInfo { get; set; }
        [Required]
        public string OtherUserContactInfo { get; set; }
        [Required]
        public ExchangeState State { get; set; }
        public DateTime FirstUserFinalizeTime { get; set; }
        public DateTime SecondUserFinalizeTime { get; set; }
    }
}

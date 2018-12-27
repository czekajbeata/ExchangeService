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
        public string FirstUsersGames { get; set; }
        [Required]
        public string OtherUsersGames { get; set; }
        [Required]
        public bool Pickup { get; set; }
        public string PickUpLocation { get; set; }
        [Required]
        public bool Delivery { get; set; }
        [Required]
        public ExchangeState State { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(512)]
        public string InnerUserId { get; set; }
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        [StringLength(128)]
        public string Surname { get; set; }
        [Required]
        public bool Pickup { get; set; }
        public string PickUpLocation { get; set; }
        [Required]
        public bool Delivery { get; set; }
        public string ImageUrl { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }

    }
}

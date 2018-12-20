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
        [StringLength(264)]
        public string NameAndSurname { get; set; }
        [Required]
        public bool Pickup { get; set; }
        public string PickUpLocation { get; set; }
        [Required]
        public bool Delivery { get; set; }

    }
}

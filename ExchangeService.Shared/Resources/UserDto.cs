using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeService.Shared.Resources
{
    public class UserDto
    {
        public int UserId { get; set; }        
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Pickup { get; set; }
        public string PickUpLocation { get; set; }
        public bool Delivery { get; set; }
        public string ImageUrl { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
    }
}

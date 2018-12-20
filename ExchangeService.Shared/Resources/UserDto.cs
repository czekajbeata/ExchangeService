using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeService.Shared.Resources
{
    public class UserDto
    {
        public int UserId { get; set; }        
        public string NameAndSurname { get; set; }
        public bool Pickup { get; set; }
        public string PickUpLocation { get; set; }
        public bool Delivery { get; set; }
    }
}

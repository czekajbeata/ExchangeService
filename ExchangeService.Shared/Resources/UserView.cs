using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeService.Shared.Resources
{
    public class UserView
    {
        public int UserId { get; set; }        
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public double AvgMark { get; set; }
        public int ReviewsCount { get; set; }
        public int ExchangesCount { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
    }
}

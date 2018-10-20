using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Controllers.Resources
{
    public class NewUserGameDto
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public State State { get; set; }
        public bool IsComplete { get; set; }
        public Shipment Shipment { get; set; }
    }
}

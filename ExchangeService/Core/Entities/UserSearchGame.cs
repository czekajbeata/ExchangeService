using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Core.Entities
{
    public class UserSearchGame
    {
        public int UserSearchGameId { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
    }
}

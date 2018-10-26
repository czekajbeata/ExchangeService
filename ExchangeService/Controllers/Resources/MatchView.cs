using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Controllers.Resources
{
    public class MatchView
    {
        public int OtherUserId { get; set; }
        public List<string> GamesTheyHave { get; set; }
        public List<string> GamesTheyWant { get; set; }
    }
}

using System.Collections.Generic;

namespace ExchangeService.Shared.Resources
{
    public class MatchView
    {
        public int OtherUserId { get; set; }
        public List<string> GamesTheyHave { get; set; }
        public List<string> GamesTheyWant { get; set; }
    }
}

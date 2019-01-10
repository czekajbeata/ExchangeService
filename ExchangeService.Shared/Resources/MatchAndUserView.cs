using System.Collections.Generic;

namespace ExchangeService.Shared.Resources
{
    public class MatchAndUserView
    {
        public int OtherUserId { get; set; }
        public string UserImageUrl { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public double AvgMark { get; set; }
        public string[] GamesTheyHave { get; set; }
        public string[] GamesTheyWant { get; set; }
    }
}

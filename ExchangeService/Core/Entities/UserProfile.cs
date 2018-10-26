using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Core.Entities
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<UserGame> ForExchange { get; set; }
        public IEnumerable<UserSearchGame> Seached { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}

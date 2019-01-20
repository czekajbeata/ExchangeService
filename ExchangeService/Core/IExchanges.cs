using ExchangeService.Core.Entities;
using System.Collections.Generic;

namespace ExchangeService.Core
{
    public interface IExchanges
    {
        IEnumerable<Exchange> GetUserExchanges(int userId);
        Exchange GetExchange(int exchangeId);
        Exchange AddExchange(Exchange newExchange);
        Comment GetCommentByExchange(int exchangeId, int receivingUserId);
        void RemoveExchangeGames(int otherUserId, string otherUsersGames);
        void DeclineWaitingExchanges(int offeringUserId, string offeringUsersGames, int exchangeId);
    }
}

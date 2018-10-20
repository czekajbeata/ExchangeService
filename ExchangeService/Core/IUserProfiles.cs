using ExchangeService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Core
{
    public interface IUserProfiles
    {
        UserGame AddGame(UserGame game);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Core
{
    public interface IJwtTokenService
    {
        string BuildToken(string email);
    }
}

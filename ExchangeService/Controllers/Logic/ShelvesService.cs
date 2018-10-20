using ExchangeService.Controllers.Resources;
using ExchangeService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Controllers.Logic
{
    public class ShelvesService
    {
        private readonly IGames games;
        private readonly IUserProfiles userProfiles;
        private readonly IUnitOfWork unitOfWork;

        public ShelvesService(IGames games, IUserProfiles userProfiles, IUnitOfWork unitOfWork)
        {
            this.games = games;
            this.userProfiles = userProfiles;
            this.unitOfWork = unitOfWork;
        }

        public bool AddUserGame(NewUserGameDto newUserGame)
        {
            throw new NotImplementedException();
        }

        public bool AddGame(GameDto game)
        {
            throw new NotImplementedException();
        }

        public GameDto GetGameDetails(int id)
        {
            throw new NotImplementedException();
        }
    }
}

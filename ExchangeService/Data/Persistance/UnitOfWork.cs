using ExchangeService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Data.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void CompleteWork()
        {
            context.SaveChanges();
        }
    }
}

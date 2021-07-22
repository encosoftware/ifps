using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENCO.DDD.UoW;
using Microsoft.EntityFrameworkCore;

namespace ENCO.DDD.EntityFrameworkCore.Relational.UoW
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public EfCoreUnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}

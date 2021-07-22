using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class StorageRepository : EFCoreRepositoryBase<IFPSFactoryContext, Storage>, IStorageRepository
    {
        public StorageRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Storage, object>>> DefaultIncludes => new List<Expression<Func<Storage, object>>>
        {

        };

        public Task<Storage> GetStorageWithIncludesAsync(int id)
        {
            return GetAll()
                .Include(ent => ent.StorageCells)
                    .ThenInclude(ent => ent.Stocks)
                .SingleAsync(ent => ent.Id == id);
        }
    }
}

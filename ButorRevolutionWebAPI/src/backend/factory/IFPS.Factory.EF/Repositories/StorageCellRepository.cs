using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class StorageCellRepository : EFCoreRepositoryBase<IFPSFactoryContext, StorageCell>, IStorageCellRepository
    {
        public StorageCellRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<StorageCell, object>>> DefaultIncludes => new List<Expression<Func<StorageCell, object>>>
        {

        };

        public Task<List<Tuple<Guid, int>>> GetMaterialStock(List<Guid> matIds)
        {
            var result = context.Stocks
                .Where(ent => matIds.Contains(ent.Package.MaterialId))
                .GroupBy(ent => ent.Package.MaterialId)
                .Select(ent => Tuple.Create<Guid,int>(ent.Key, ent.Sum(st => st.Quantity * st.Package.Size) ))
                .ToList();

            return Task.FromResult(result);
        }

    }
}

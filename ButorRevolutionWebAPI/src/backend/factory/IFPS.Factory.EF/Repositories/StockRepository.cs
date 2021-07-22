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
    public class StockRepository : EFCoreRepositoryBase<IFPSFactoryContext, Stock>, IStockRepository
    {
        public StockRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Stock, object>>> DefaultIncludes => new List<Expression<Func<Stock, object>>> { };

        public Task<MaterialPackage> GetMachineStoredMaterialPackage(int storageCellId, Guid materialId)
        {
            var result = GetAll()
                .Where(ent => ent.StorageCellId == storageCellId && ent.Package.MaterialId == materialId)
                .Select(ent => ent.Package)
                .SingleOrDefault();

            return Task.FromResult(result);
        }
    }
}

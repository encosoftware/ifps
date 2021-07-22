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
    public class StockedMaterialRepository : EFCoreRepositoryBase<IFPSFactoryContext, StockedMaterial>, IStockedMaterialRepository
    {
        public StockedMaterialRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<StockedMaterial, object>>> DefaultIncludes => new List<Expression<Func<StockedMaterial, object>>>
        {

        };

        public Task<List<StockedMaterial>> GetStockedMaterialsByIds(List<Guid> matIds)
        {
            var result = GetAll()
                .Where(ent => matIds.Contains(ent.MaterialId))
                .ToList();

            return Task.FromResult(result);
        }
    }
}

using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class ApplianceMaterialRepository : EFCoreRepositoryBase<IFPSSalesContext, ApplianceMaterial, Guid>, IApplianceMaterialRepository
    {
        public ApplianceMaterialRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<ApplianceMaterial, object>>> DefaultIncludes =>
            new List<Expression<Func<ApplianceMaterial, object>>>
        {
            ent => ent.Translations
        };

        public async Task<IPagedList<ApplianceMaterial>> GetApplianceMaterialsAsync(Expression<Func<ApplianceMaterial, bool>> predicate = null,
            Func<IQueryable<ApplianceMaterial>, IOrderedQueryable<ApplianceMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.Brand)
                .Include(ent => ent.Image)
                .Include(ent => ent.CurrentPrice)
                    .ThenInclude(ent => ent.Price)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.Category)
                    .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.SellPrice)
                    .ThenInclude(ent => ent.Currency);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}

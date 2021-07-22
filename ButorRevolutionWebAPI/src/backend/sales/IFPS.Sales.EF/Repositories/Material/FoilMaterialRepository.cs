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
    public class FoilMaterialRepository : EFCoreRepositoryBase<IFPSSalesContext, FoilMaterial, Guid>, IFoilMaterialRepository
    {
        public FoilMaterialRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<FoilMaterial, object>>> DefaultIncludes =>
            new List<Expression<Func<FoilMaterial, object>>>
        {
            ent => ent.Translations
        };

        public async Task<IPagedList<FoilMaterial>> GetFoilMaterialsAsync(Expression<Func<FoilMaterial, bool>> predicate = null,
            Func<IQueryable<FoilMaterial>, IOrderedQueryable<FoilMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.CurrentPrice)
                    .ThenInclude(ent => ent.Price)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.Image)
                .Include(ent => ent.Category)
                    .ThenInclude(ent => ent.Translations);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}

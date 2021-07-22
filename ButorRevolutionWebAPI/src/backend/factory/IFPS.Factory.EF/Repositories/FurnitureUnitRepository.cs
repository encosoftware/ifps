using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IFPS.Factory.EF.Repositories
{
    public class FurnitureUnitRepository : EFCoreRepositoryBase<IFPSFactoryContext, FurnitureUnit, Guid>, IFurnitureUnitRepository
    {
        public FurnitureUnitRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<FurnitureUnit, object>>> DefaultIncludes => new List<Expression<Func<FurnitureUnit, object>>>
        {
            ent => ent.Translations
        };

        public async Task<FurnitureUnit> GetFurnitureUnitAsync(Guid id)
        {
            return await GetAll()
                .Include(ent => ent.CurrentPrice)
                .Include(ent => ent.Components)
                    .ThenInclude(ent => ent.BoardMaterial)
                .Include(ent => ent.Components)
                    .ThenInclude(ent => ent.Image)
                .Include(ent => ent.Components)
                    .ThenInclude(ent => ent.BottomFoil)
                .Include(ent => ent.Components)
                    .ThenInclude(ent => ent.TopFoil)
                .Include(ent => ent.Components)
                    .ThenInclude(ent => ent.RightFoil)
                .Include(ent => ent.Components)
                    .ThenInclude(ent => ent.LeftFoil)
                .Include(ent => ent.Accessories)
                    .ThenInclude(ent => ent.Accessory.Image)
                .Include(ent => ent.Translations)
                .Include(ent => ent.Image)
                .Include(ent => ent.Category)

                .SingleOrDefaultAsync(ent => ent.Id == id);
        }

        public async Task<IPagedList<FurnitureUnit>> GetFurnitureUnitsAsync(
            Expression<Func<FurnitureUnit, bool>> predicate = null,
            Func<IQueryable<FurnitureUnit>, IOrderedQueryable<FurnitureUnit>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.CurrentPrice)
                    .ThenInclude(ent => ent.Price)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.CurrentPrice)
                    .ThenInclude(ent => ent.MaterialCost)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.Components)
                    .ThenInclude(ent => ent.Image)
                .Include(ent => ent.Image)
                .Include(ent => ent.Category)
                    .ThenInclude(ent => ent.Translations);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IFPS.Sales.EF.Repositories
{
    public class FurnitureUnitRepository : EFCoreRepositoryBase<IFPSSalesContext, FurnitureUnit, Guid>, IFurnitureUnitRepository
    {
        public FurnitureUnitRepository(IFPSSalesContext context) : base(context)
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

        public async Task<FurnitureUnit> GetFurnitureUnitByIdByOfferAsync(Guid furnitureUnitId)
        {
            return await GetAll()
                .Include(ent => ent.CurrentPrice)
                .Include(ent => ent.Accessories)
                    .ThenInclude(ent => ent.Accessory)
                        .ThenInclude(ent => ent.CurrentPrice)
                            .ThenInclude(ent => ent.Price)
                .SingleAsync(ent => ent.Id == furnitureUnitId);
        }

        public async Task<IPagedList<FurnitureUnit>> GetFurnitureUnitsAsync(Expression<Func<FurnitureUnit, bool>> predicate = null,
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

        public Task<List<FurnitureUnit>> GetFurnitureUnitsByType(FurnitureUnitTypeEnum type)
        {
            return GetAll()
                .Where(ent => ent.FurnitureUnitType.Type == type)
                .ToListAsync();
        }

        public async Task<List<FurnitureUnit>> GetTrendingProductsAsync()
        {
            return await GetAll()
                   .Where(ent => ent.Trending)
                   .Include(ent => ent.CurrentPrice)
                        .ThenInclude(ent => ent.Price)
                            .ThenInclude(ent => ent.Currency)
                   .Include(ent => ent.Image)
                   .ToListAsync();
        }

        public async Task<bool> IsFurnitureUnitExistedAsync(string code)
        {
            var furnitureUnit = await GetAll()
                .Where(ent => ent.Code.Equals(code))
                .CountAsync();

            return furnitureUnit > 0;
        }
    }
}


using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using IFPS.Sales.Domain.Enums;
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
    public class WebshopFurnitureUnitRepository : EFCoreRepositoryBase<IFPSSalesContext, WebshopFurnitureUnit>, IWebshopFurnitureUnitRepository
    {
        public WebshopFurnitureUnitRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<WebshopFurnitureUnit, object>>> DefaultIncludes => new List<Expression<Func<WebshopFurnitureUnit, object>>>
        { };

        public async Task<IPagedList<WebshopFurnitureUnit>> GetWebshopFurnitureUnits(
            Expression<Func<WebshopFurnitureUnit, bool>> predicate = null, 
            Func<IQueryable<WebshopFurnitureUnit>, 
                IOrderedQueryable<WebshopFurnitureUnit>> orderBy = null, 
            int pageIndex = 0, 
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.FurnitureUnit)
                .Include(ent => ent.Price)
                    .ThenInclude(price => price.Currency);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<IPagedList<WebshopFurnitureUnit>> GetWebshopFurnitureUnitsBySearch(
            Expression<Func<WebshopFurnitureUnit, bool>> predicate = null,
            Func<IQueryable<WebshopFurnitureUnit>,
                IOrderedQueryable<WebshopFurnitureUnit>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.FurnitureUnit)
                    .ThenInclude(fu => fu.Image)
                .Include(ent => ent.Price)
                    .ThenInclude(price => price.Currency);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<IPagedList<WebshopFurnitureUnit>> GetWebshopFurnitureUnitsByCategoryTypeAsync(GroupingCategoryEnum type, 
            Expression<Func<WebshopFurnitureUnit, bool>> predicate = null, 
            Func<IQueryable<WebshopFurnitureUnit>, 
            IOrderedQueryable<WebshopFurnitureUnit>> orderBy = null, 
            int pageIndex = 0, 
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.FurnitureUnit)
                    .ThenInclude(fu => fu.Category)
                .Include(ent => ent.FurnitureUnit)
                    .ThenInclude(fu => fu.Image)
                .Include(ent => ent.Price)
                    .ThenInclude(price => price.Currency)
                .Include(ent => ent.Images)
                .Where(ent => ent.FurnitureUnit.Category.CategoryType == type);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<WebshopFurnitureUnit> GetWfuById(int id)
        {
            return await GetAll()
                .Include(ent => ent.FurnitureUnit)
                .Include(ent => ent.Images)
                    .ThenInclude(image => image.Image)
                .Include(ent => ent.Price)
                    .ThenInclude(price => price.Currency)
                .SingleAsync(ent => ent.Id == id);
        }

        public async Task<WebshopFurnitureUnit> GetWebshopFurnitureUnitByWebshopByIdAsync(int webshopFurnitureUnitId)
        {
            return await GetAll()
                .Include(ent => ent.FurnitureUnit)
                    .ThenInclude(ent => ent.Image)
                .Include(ent => ent.Images)
                    .ThenInclude(image => image.Image)
                .Include(ent => ent.Price)
                    .ThenInclude(price => price.Currency)
                .SingleAsync(ent => ent.Id == webshopFurnitureUnitId);
        }
    }
}

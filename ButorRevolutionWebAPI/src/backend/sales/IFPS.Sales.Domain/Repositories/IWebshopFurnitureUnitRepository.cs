using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IWebshopFurnitureUnitRepository : IRepository<WebshopFurnitureUnit>
    {
        Task<IPagedList<WebshopFurnitureUnit>> GetWebshopFurnitureUnits(
            Expression<Func<WebshopFurnitureUnit, bool>> predicate = null,
            Func<IQueryable<WebshopFurnitureUnit>, IOrderedQueryable<WebshopFurnitureUnit>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<IPagedList<WebshopFurnitureUnit>> GetWebshopFurnitureUnitsBySearch(
            Expression<Func<WebshopFurnitureUnit, bool>> predicate = null,
            Func<IQueryable<WebshopFurnitureUnit>,
                IOrderedQueryable<WebshopFurnitureUnit>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<IPagedList<WebshopFurnitureUnit>> GetWebshopFurnitureUnitsByCategoryTypeAsync(GroupingCategoryEnum type,
            Expression<Func<WebshopFurnitureUnit, bool>> predicate = null,
            Func<IQueryable<WebshopFurnitureUnit>, IOrderedQueryable<WebshopFurnitureUnit>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<WebshopFurnitureUnit> GetWfuById(int id);
        Task<WebshopFurnitureUnit> GetWebshopFurnitureUnitByWebshopByIdAsync(int webshopFurnitureUnitId);
    }
}

using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IFurnitureUnitRepository : IRepository<FurnitureUnit, Guid>
    {
        Task<FurnitureUnit> GetFurnitureUnitAsync(Guid id);        
        Task<IPagedList<FurnitureUnit>> GetFurnitureUnitsAsync(Expression<Func<FurnitureUnit, bool>> predicate = null,
            Func<IQueryable<FurnitureUnit>, IOrderedQueryable<FurnitureUnit>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<List<FurnitureUnit>> GetFurnitureUnitsByType(FurnitureUnitTypeEnum type);
        Task<List<FurnitureUnit>> GetTrendingProductsAsync();
        Task<FurnitureUnit> GetFurnitureUnitByIdByOfferAsync(Guid furnitureUnitId);
        Task<bool> IsFurnitureUnitExistedAsync(string code);
    }
}

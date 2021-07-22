using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IFurnitureUnitRepository : IRepository<FurnitureUnit, Guid>
    {
        Task<FurnitureUnit> GetFurnitureUnitAsync(Guid id);

        Task<IPagedList<FurnitureUnit>> GetFurnitureUnitsAsync(Expression<Func<FurnitureUnit, bool>> predicate = null,
            Func<IQueryable<FurnitureUnit>, IOrderedQueryable<FurnitureUnit>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}
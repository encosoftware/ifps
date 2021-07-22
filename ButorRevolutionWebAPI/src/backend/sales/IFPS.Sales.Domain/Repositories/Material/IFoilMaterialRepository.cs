using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IFoilMaterialRepository : IRepository<FoilMaterial, Guid>
    {
        Task<IPagedList<FoilMaterial>> GetFoilMaterialsAsync(Expression<Func<FoilMaterial, bool>> predicate = null,
            Func<IQueryable<FoilMaterial>, IOrderedQueryable<FoilMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}

using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IAccessoryMaterialRepository : IRepository<AccessoryMaterial, Guid>
    {
        Task<IPagedList<AccessoryMaterial>> GetAccessoryMaterialsAsync(Expression<Func<AccessoryMaterial, bool>> predicate = null,
            Func<IQueryable<AccessoryMaterial>, IOrderedQueryable<AccessoryMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}

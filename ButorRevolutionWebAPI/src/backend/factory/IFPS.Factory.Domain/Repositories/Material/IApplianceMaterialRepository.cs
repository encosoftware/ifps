using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IApplianceMaterialRepository : IRepository<ApplianceMaterial, Guid>
    {
        Task<IPagedList<ApplianceMaterial>> GetApplianceMaterialsAsync(Expression<Func<ApplianceMaterial, bool>> predicate = null,
            Func<IQueryable<ApplianceMaterial>, IOrderedQueryable<ApplianceMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IOptimizationRepository : IRepository<Optimization, Guid>
    {
        Task<IPagedList<Optimization>> GetOptimizationsAsync(Expression<Func<Optimization, bool>> predicate = null,
            Func<IQueryable<Optimization>, IOrderedQueryable<Optimization>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}

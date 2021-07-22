using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IWorktopBoardMaterialRepository : IRepository<WorktopBoardMaterial, Guid>
    {
        Task<IPagedList<WorktopBoardMaterial>> GetWorktopBoardMaterialsAsync(Expression<Func<WorktopBoardMaterial, bool>> predicate = null,
            Func<IQueryable<WorktopBoardMaterial>, IOrderedQueryable<WorktopBoardMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}

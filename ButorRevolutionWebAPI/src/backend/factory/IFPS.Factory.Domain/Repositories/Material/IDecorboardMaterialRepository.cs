using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IDecorBoardMaterialRepository : IRepository<DecorBoardMaterial, Guid>
    {
        Task<IPagedList<DecorBoardMaterial>> GetDecorBoardMaterialsAsync(Expression<Func<DecorBoardMaterial, bool>> predicate = null,
            Func<IQueryable<DecorBoardMaterial>, IOrderedQueryable<DecorBoardMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}
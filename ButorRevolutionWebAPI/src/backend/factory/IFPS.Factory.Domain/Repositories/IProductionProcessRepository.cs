using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IProductionProcessRepository : IRepository<ProductionProcess>
    {
        Task<IPagedList<ProductionProcess>> GetPagedProductionProcessAsync
            (Type type,
            Expression<Func<ProductionProcess, bool>> predicate = null,
            Func<IQueryable<ProductionProcess>, IOrderedQueryable<ProductionProcess>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<IPagedList<ProductionProcess>> GetAssemblyManualProductionProcessesAsync
            (Expression<Func<ProductionProcess, bool>> predicate = null,
            Func<IQueryable<ProductionProcess>, IOrderedQueryable<ProductionProcess>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<IPagedList<ProductionProcess>> GetSortingManualProductionProcessesAsync
            (Expression<Func<ProductionProcess, bool>> predicate = null,
            Func<IQueryable<ProductionProcess>, IOrderedQueryable<ProductionProcess>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<IPagedList<ProductionProcess>> GetPackingManualProductionProcessesAsync
            (Expression<Func<ProductionProcess, bool>> predicate = null,
            Func<IQueryable<ProductionProcess>, IOrderedQueryable<ProductionProcess>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}

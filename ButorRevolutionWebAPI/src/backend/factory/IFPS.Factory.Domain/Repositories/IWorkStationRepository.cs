using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IWorkStationRepository : IRepository<WorkStation>
    {
        Task<IPagedList<WorkStation>> GetPagedWorkStationsAsync
            (Expression<Func<WorkStation, bool>> predicate = null,
            Func<IQueryable<WorkStation>, IOrderedQueryable<WorkStation>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
        Task<WorkStation> GetWorkStationsWithCameras(int workStationId);
    }
}

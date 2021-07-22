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
    public interface IMachineRepository : IRepository<Machine>
    {
        Task<IPagedList<Machine>> GetMachinesAsync(Expression<Func<Machine, bool>> predicate = null,
           Func<IQueryable<Machine>, IOrderedQueryable<Machine>> orderBy = null,
           int pageIndex = 0,
           int pageSize = 20);

        Task<List<Machine>> GetMachinesByType(Type type);
    }
}

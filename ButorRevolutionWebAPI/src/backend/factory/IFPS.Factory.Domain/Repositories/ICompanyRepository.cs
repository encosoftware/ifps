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
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<IPagedList<Company>> GetCompaniesWithIncludesAsync(Expression<Func<Company, bool>> predicate = null,
            Func<IQueryable<Company>, IOrderedQueryable<Company>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<Company> GetCompanyByIdAsync(int id);
        Task<List<Company>> GetSuppliersListAsync();
    }
}

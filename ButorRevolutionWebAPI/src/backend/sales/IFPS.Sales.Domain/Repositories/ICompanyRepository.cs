using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<IPagedList<Company>> GetCompaniesWithIncludesAsync(Expression<Func<Company, bool>> predicate = null,
            Func<IQueryable<Company>, IOrderedQueryable<Company>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
        Task<Company> GetCompanyAsync(int id);
    }
}

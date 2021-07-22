using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IVenueRepository : IRepository<Venue>
    {
        Task<List<Venue>> GetVenuesAsync(Expression<Func<Venue, bool>> predicate, int take = 10);

        Task<Venue> GetVenueAsync(int venueId);
        Task<IPagedList<Venue>> GetPagedVenuesAsync(Expression<Func<Venue, bool>> predicate = null,
            Func<IQueryable<Venue>, IOrderedQueryable<Venue>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
        Task<List<Venue>> GetVenuesByCompanyAsync(int companyId);
    }
}

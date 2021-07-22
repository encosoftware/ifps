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
    public interface IRequiredMaterialsRepository : IRepository<RequiredMaterial>
    {
        Task <IPagedList<RequiredMaterial>> GetPagedRequiredMaterialAsync
            (Expression<Func<RequiredMaterial, bool>> predicate = null,
            Func<IQueryable<RequiredMaterial>, IOrderedQueryable<RequiredMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task <List<RequiredMaterial>> GetRequiredMaterialsWithIncludeAsync(List<int> ids);

        Task<List<RequiredMaterial>> GetMaterialsForCodeListAsync();
    }
}

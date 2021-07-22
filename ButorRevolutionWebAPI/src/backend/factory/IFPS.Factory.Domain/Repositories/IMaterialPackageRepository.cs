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
    public interface IMaterialPackageRepository : IRepository<MaterialPackage>
    {
        Task<List<MaterialPackage>> GetMaterialPackegesAsync(int supplierId);
        Task<IPagedList<MaterialPackage>> GetMaterialPackagePagedListAsync(
            Expression<Func<MaterialPackage, bool>> predicate = null,
            Func<IQueryable<MaterialPackage>,
            IOrderedQueryable<MaterialPackage>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}

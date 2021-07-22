using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface ICargoRepository : IRepository<Cargo>
    {
        Task<IPagedList<Cargo>> GetPagedCargoAsync
            (List<CargoStatusEnum> statuses,
            Expression<Func<Cargo, bool>> predicate = null,
            Func<IQueryable<Cargo>, IOrderedQueryable<Cargo>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);

        Task<Cargo> GetCargoByIdWithIncludes(int cargoId);
    }
}

using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class OptimizationRepository : EFCoreRepositoryBase<IFPSFactoryContext, Optimization, Guid>, IOptimizationRepository
    {
        public OptimizationRepository(IFPSFactoryContext context) : base(context)
        {

        }

        protected override List<Expression<Func<Optimization, object>>> DefaultIncludes => new List<Expression<Func<Optimization, object>>>
        {

        };

        public async Task<IPagedList<Optimization>> GetOptimizationsAsync(Expression<Func<Optimization, bool>> predicate = null,
            Func<IQueryable<Optimization>, IOrderedQueryable<Optimization>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll();

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}

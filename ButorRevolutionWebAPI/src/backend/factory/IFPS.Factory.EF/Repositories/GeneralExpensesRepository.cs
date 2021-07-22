using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
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
    public class GeneralExpensesRepository : EFCoreRepositoryBase<IFPSFactoryContext, GeneralExpense>, IGeneralExpenseRepository
    {
        public GeneralExpensesRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<GeneralExpense, object>>> DefaultIncludes => new List<Expression<Func<GeneralExpense, object>>>
        {

        };

        public async Task<IPagedList<GeneralExpense>> GetGeneralExpensesWithIncludesAsync(Expression<Func<GeneralExpense, bool>> predicate = null,
            Func<IQueryable<GeneralExpense>, IOrderedQueryable<GeneralExpense>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.Cost)
                    .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.GeneralExpenseRule);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}

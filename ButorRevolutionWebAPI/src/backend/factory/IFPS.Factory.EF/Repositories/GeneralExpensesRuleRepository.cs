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
    public class GeneralExpensesRuleRepository : EFCoreRepositoryBase<IFPSFactoryContext, GeneralExpenseRule>, IGeneralExpenseRuleRepository
    {
        public GeneralExpensesRuleRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<GeneralExpenseRule, object>>> DefaultIncludes => new List<Expression<Func<GeneralExpenseRule, object>>>
        {

        };

        public async Task<IPagedList<GeneralExpenseRule>> GetGeneralExpenseRulesWithIncludesAsync(Expression<Func<GeneralExpenseRule, bool>> predicate = null,
            Func<IQueryable<GeneralExpenseRule>, IOrderedQueryable<GeneralExpenseRule>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.Amount)
                    .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.FrequencyType)
                    .ThenInclude(ent => ent.Translations);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}

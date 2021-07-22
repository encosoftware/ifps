using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IGeneralExpenseRuleRepository : IRepository<GeneralExpenseRule>
    {
        Task<IPagedList<GeneralExpenseRule>> GetGeneralExpenseRulesWithIncludesAsync(Expression<Func<GeneralExpenseRule, bool>> predicate = null,
            Func<IQueryable<GeneralExpenseRule>, IOrderedQueryable<GeneralExpenseRule>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}

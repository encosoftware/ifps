using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IGeneralExpenseRepository : IRepository<GeneralExpense>
    {
        Task<IPagedList<GeneralExpense>> GetGeneralExpensesWithIncludesAsync(Expression<Func<GeneralExpense, bool>> predicate = null,
            Func<IQueryable<GeneralExpense>, IOrderedQueryable<GeneralExpense>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20);
    }
}

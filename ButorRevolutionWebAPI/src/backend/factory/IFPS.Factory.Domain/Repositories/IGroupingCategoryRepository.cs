using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IGroupingCategoryRepository : IRepository<GroupingCategory>
    {
        Task<List<GroupingCategory>> GetAllListIncludingAsync(Expression<Func<GroupingCategory, bool>> predicate);
        
    }
}
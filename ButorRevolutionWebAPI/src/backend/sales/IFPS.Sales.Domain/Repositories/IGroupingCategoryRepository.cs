using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IGroupingCategoryRepository : IRepository<GroupingCategory>
    {
        Task<List<GroupingCategory>> GetAllListIncludingAsync(Expression<Func<GroupingCategory, bool>> predicate);
        Task<List<GroupingCategory>> GetWebshopCategoriesByParentEnumTypeAsync(GroupingCategoryEnum enumType);
        Task<GroupingCategory> GetGroupingCategoryByName(string name);
        Task<List<GroupingCategory>> GetAllListForDropdownAsync(GroupingCategoryEnum type);
        Task<List<GroupingCategory>> GetAllDecorBoardCategoryAsync();
    }
}

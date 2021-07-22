using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class GroupingCategoryRepository : EFCoreRepositoryBase<IFPSSalesContext, GroupingCategory>, IGroupingCategoryRepository
    {
        public GroupingCategoryRepository(IFPSSalesContext context) : base(context)
        {
        }

        protected override List<Expression<Func<GroupingCategory, object>>> DefaultIncludes => new List<Expression<Func<GroupingCategory, object>>>
        {
            ent => ent.Translations
        };

        public Task<List<GroupingCategory>> GetAllListIncludingAsync(Expression<Func<GroupingCategory, bool>> predicate)
        {
            return GetAll()
                .Where(predicate)                
                .Include(ent => ent.Children)
                    .ThenInclude(child => child.Translations)
                .Include(ent => ent.Children)
                    .ThenInclude(child => child.Children)
                        .ThenInclude(grandchild => grandchild.Translations)
                .ToListAsync();
        }

        public Task<List<GroupingCategory>> GetWebshopCategoriesByParentEnumTypeAsync(GroupingCategoryEnum enumType)
        {
            return GetAll()
                .Include(ent => ent.Children)
                    .ThenInclude(child => child.Translations)
                .Include(ent => ent.Children)
                    .ThenInclude(child => child.Image)
                .Include(ent => ent.Image)
                .Include(ent => ent.ParentGroup)
                .Where(ent => ent.ParentGroup.CategoryType == enumType)
                .ToListAsync();
        }

        public Task<GroupingCategory> GetGroupingCategoryByName(string name)
        {
            return GetAll()
                .Include(ent => ent.Translations)
                .Where(ent => ent.Translations.Any(t => t.GroupingCategoryName.Equals(name)))
                .SingleOrDefaultAsync();
        }

        public Task<List<GroupingCategory>> GetAllListForDropdownAsync(GroupingCategoryEnum type)
        {
            return GetAll()
                .Where(ent => ent.CategoryType == type && ent.ParentGroupId.HasValue)
                .Include(ent => ent.Children)
                    .ThenInclude(child => child.Translations)
                .Include(ent => ent.ParentGroup)
                    .ThenInclude(ent => ent.Translations)
                .ToListAsync();
        }

        public async Task<List<GroupingCategory>> GetAllDecorBoardCategoryAsync()
        {
            return await GetAll()
                .Include(ent => ent.Translations)
                .Where(ent => ent.CategoryType == GroupingCategoryEnum.DecorBoard && ent.ParentGroupId.HasValue &&
                        ent.Children.Count() == 0)
                .ToListAsync();
        }
    }
}

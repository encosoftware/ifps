using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
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
    public class GroupingCategoryRepository : EFCoreRepositoryBase<IFPSFactoryContext, GroupingCategory>, IGroupingCategoryRepository
    {
        public GroupingCategoryRepository(IFPSFactoryContext context) : base(context)
        {
        }

        protected override List<Expression<Func<GroupingCategory, object>>> DefaultIncludes => new List<Expression<Func<GroupingCategory, object>>>
        {
            p => p.Translations
        };



        public Task<List<GroupingCategory>> GetAllListIncludingAsync(Expression<Func<GroupingCategory, bool>> predicate)
        {
            return GetAll()
                .Include(ent => ent.Children)
                    .ThenInclude(child => child.Translations)
                .Where(predicate)
                .ToListAsync();
        }
    }
}
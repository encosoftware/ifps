using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IFPS.Factory.EF.Repositories
{
    public class SortingStrategyTypeRepository : EFCoreRepositoryBase<IFPSFactoryContext, SortingStrategyType>, ISortingStrategyTypeRepository
    {
        public SortingStrategyTypeRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<SortingStrategyType, object>>> DefaultIncludes => new List<Expression<Func<SortingStrategyType, object>>>
        {
            ent => ent.Translations
        };

        public async Task<SortingStrategyType> GetSortingStrategyTypeById(int id)
        {
            return await GetAll()
                .SingleAsync(ent => ent.Id == id);
        }
    }
}

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
    public class DecorBoardMaterialRepository : EFCoreRepositoryBase<IFPSFactoryContext, DecorBoardMaterial, Guid>, IDecorBoardMaterialRepository
    {
        public DecorBoardMaterialRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<DecorBoardMaterial, object>>> DefaultIncludes =>
            new List<Expression<Func<DecorBoardMaterial, object>>>
        {
            ent => ent.Translations
        };

        public async Task<IPagedList<DecorBoardMaterial>> GetDecorBoardMaterialsAsync(Expression<Func<DecorBoardMaterial, bool>> predicate = null,
            Func<IQueryable<DecorBoardMaterial>, IOrderedQueryable<DecorBoardMaterial>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.CurrentPrice)
                    .ThenInclude(ent => ent.Price)
                        .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.Image)
                .Include(ent => ent.Category)
                    .ThenInclude(ent => ent.Translations);

            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}
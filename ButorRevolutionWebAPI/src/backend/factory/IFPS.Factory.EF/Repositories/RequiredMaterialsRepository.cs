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
    public class RequiredMaterialsRepository : EFCoreRepositoryBase<IFPSFactoryContext, RequiredMaterial>, IRequiredMaterialsRepository
    {
        public RequiredMaterialsRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<RequiredMaterial, object>>> DefaultIncludes => new List<Expression<Func<RequiredMaterial, object>>> { };

        public async Task<IPagedList<RequiredMaterial>> GetPagedRequiredMaterialAsync
            (Expression<Func<RequiredMaterial, bool>> predicate = null, 
            Func<IQueryable<RequiredMaterial>, IOrderedQueryable<RequiredMaterial>> orderBy = null, 
            int pageIndex = 0, 
            int pageSize = 20)
        {
            var requiredMaterials = GetAll()
                .Include(ent => ent.Order)
                .Include(ent => ent.Material)
                    .ThenInclude(material => material.SiUnit)
                .Include(ent => ent.Material)
                    .ThenInclude(material => material.Packages)
                        .ThenInclude(package => package.Supplier);

            return await GetPagedListAsync(requiredMaterials, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<List<RequiredMaterial>> GetRequiredMaterialsWithIncludeAsync(List<int> ids)
        {
            var result = await GetAll()                
                .Include(ent => ent.Order)
                .Include(ent => ent.Material)
                    .ThenInclude(ent => ent.Packages)
                        .ThenInclude(package => package.Price)
                            .ThenInclude(price => price.Currency)
                .Include(ent => ent.Material)
                    .ThenInclude(ent => ent.Packages)
                        .ThenInclude(package => package.Supplier)
                .Where(ent => ids.Contains(ent.Id))
                .ToListAsync();

            return result;
        }

        public async Task<List<RequiredMaterial>> GetMaterialsForCodeListAsync()
        {
            return await GetAll()
                .Include(entity => entity.Material)
                .GroupBy(ent => ent.Material.Code)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();
        }
    }
}

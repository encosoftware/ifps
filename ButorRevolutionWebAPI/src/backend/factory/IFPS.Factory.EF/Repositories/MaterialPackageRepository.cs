using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class MaterialPackageRepository : EFCoreRepositoryBase<IFPSFactoryContext, MaterialPackage>, IMaterialPackageRepository
    {
        public MaterialPackageRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<MaterialPackage, object>>> DefaultIncludes => new List<Expression<Func<MaterialPackage, object>>> { };

        public async Task<List<MaterialPackage>> GetMaterialPackegesAsync(int supplierId)
        {
            return await GetAll()
                .Include(ent => ent.Material)
                .Include(ent => ent.Supplier)
                .Where(ent => ent.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<IPagedList<MaterialPackage>> GetMaterialPackagePagedListAsync(
            Expression<Func<MaterialPackage, bool>> predicate = null, 
            Func<IQueryable<MaterialPackage>, 
            IOrderedQueryable<MaterialPackage>> orderBy = null, 
            int pageIndex = 0, 
            int pageSize = 20)
        {
            var query = GetAll()
                .Include(ent => ent.Material)
                .Include(ent => ent.Supplier)
                .Include(ent => ent.Price)
                    .ThenInclude(price => price.Currency);


            return await GetPagedListAsync(query, predicate, orderBy, pageIndex, pageSize);
        }
    }
}

using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
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
    public class OrderedMaterialRepository : EFCoreRepositoryBase<IFPSFactoryContext, OrderedMaterialPackage>, IOrderedMaterialPackageRepository
    {
        public OrderedMaterialRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<OrderedMaterialPackage, object>>> DefaultIncludes => new List<Expression<Func<OrderedMaterialPackage, object>>> { };


        public async Task<List<OrderedMaterialPackage>> GetOrderedMaterialPackagesWithInclude(int supplierId)
        {
            return await GetAll()
                .Include(ent => ent.MaterialPackage)
                    .ThenInclude(package => package.Material)
                .Include(ent => ent.MaterialPackage)
                    .ThenInclude(package => package.Supplier)
                .Include(ent => ent.Cargo)
                .Where(ent => ent.MaterialPackage.SupplierId == supplierId)
                .ToListAsync();
        }

    }
}

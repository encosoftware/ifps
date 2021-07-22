using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class AccessoryFurnitureUnitRepository : EFCoreRepositoryBase<IFPSSalesContext, AccessoryMaterialFurnitureUnit>, IAccessoryFurnitureUnitRepository
    {
        public AccessoryFurnitureUnitRepository(IFPSSalesContext context) : base(context)
        {
        }

        protected override List<Expression<Func<AccessoryMaterialFurnitureUnit, object>>> DefaultIncludes => new List<Expression<Func<AccessoryMaterialFurnitureUnit, object>>>
        {
        };
    }
}

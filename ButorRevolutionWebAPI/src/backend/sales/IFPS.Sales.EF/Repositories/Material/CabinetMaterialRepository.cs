using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class CabinetMaterialRepository : EFCoreRepositoryBase<IFPSSalesContext, CabinetMaterial>, ICabinetMaterialRepository
    {
        public CabinetMaterialRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<CabinetMaterial, object>>> DefaultIncludes => new List<Expression<Func<CabinetMaterial, object>>> {  };
    }
}

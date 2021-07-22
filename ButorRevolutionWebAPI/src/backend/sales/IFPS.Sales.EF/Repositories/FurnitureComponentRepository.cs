using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;

namespace IFPS.Sales.EF.Repositories
{
    public class FurnitureComponentRepository : EFCoreRepositoryBase<IFPSSalesContext, FurnitureComponent,Guid>, IFurnitureComponentRepository
    {
        public FurnitureComponentRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<FurnitureComponent, object>>> DefaultIncludes => new List<Expression<Func<FurnitureComponent, object>>>
        {
            
        };
    }
}

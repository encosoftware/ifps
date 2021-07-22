using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class DayTypeRepository : EFCoreRepositoryBase<IFPSSalesContext, DayType>, IDayTypeRepository
    {
        public DayTypeRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<DayType, object>>> DefaultIncludes => new List<Expression<Func<DayType, object>>>
        {
            p => ((CompanyType)p.Translations),
        };

    }
}

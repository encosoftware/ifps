using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class DayTypeRepository : EFCoreRepositoryBase<IFPSFactoryContext, DayType>, IDayTypeRepository
    {
        public DayTypeRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<DayType, object>>> DefaultIncludes => new List<Expression<Func<DayType, object>>>
        {
            p => ((CompanyType)p.Translations),
        };
    }
}
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class SiUnitRepository : EFCoreRepositoryBase<IFPSFactoryContext, SiUnit>, ISiUnitRepository
    {
        public SiUnitRepository(IFPSFactoryContext context) : base(context)
        {
        }

        protected override List<Expression<Func<SiUnit, object>>> DefaultIncludes => new List<Expression<Func<SiUnit, object>>>
        {
        };
    }
}
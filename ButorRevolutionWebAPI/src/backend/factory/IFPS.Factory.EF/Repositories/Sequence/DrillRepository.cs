using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class DrillRepository : EFCoreRepositoryBase<IFPSFactoryContext, Drill>, IDrillRepository
    {
        public DrillRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Drill, object>>> DefaultIncludes => new List<Expression<Func<Drill, object>>>
        {

        };
    }
}

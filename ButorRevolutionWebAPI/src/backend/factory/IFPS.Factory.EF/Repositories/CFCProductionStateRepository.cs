using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class CFCProductionStateRepository : EFCoreRepositoryBase<IFPSFactoryContext, CFCProductionState>, ICFCProductionStateRepository
    {
        public CFCProductionStateRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<CFCProductionState, object>>> DefaultIncludes => new List<Expression<Func<CFCProductionState, object>>>
        {
            ent => ent.Translations
        };
    }
}

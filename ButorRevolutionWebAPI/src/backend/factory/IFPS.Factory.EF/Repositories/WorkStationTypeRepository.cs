using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class WorkStationTypeRepository : EFCoreRepositoryBase<IFPSFactoryContext, WorkStationType>, IWorkStationTypeRepository
    {
        public WorkStationTypeRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<WorkStationType, object>>> DefaultIncludes => new List<Expression<Func<WorkStationType, object>>>
        {
            p => p.Translations
        };

    }
}

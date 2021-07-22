using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;

namespace IFPS.Factory.EF.Repositories
{
    public class EdgingMachineRepository : EFCoreRepositoryBase<IFPSFactoryContext, EdgingMachine>, IEdgingMachineRepository
    {
        public EdgingMachineRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<EdgingMachine, object>>> DefaultIncludes => new List<Expression<Func<EdgingMachine, object>>>
        {

        };
    }
}

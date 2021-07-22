using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class CuttingMachineRepository : EFCoreRepositoryBase<IFPSFactoryContext, CuttingMachine>, ICuttingMachineRepository
    {
        public CuttingMachineRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<CuttingMachine, object>>> DefaultIncludes => new List<Expression<Func<CuttingMachine, object>>>
        {

        };      
    }
}

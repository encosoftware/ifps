using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class PlanRepository : EFCoreRepositoryBase<IFPSFactoryContext, Plan>, IPlanRepository
    {
        public PlanRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Plan, object>>> DefaultIncludes => new List<Expression<Func<Plan, object>>> { };

        public async Task<List<Plan>> GetPlansByConcreteId(List<int> concreteFurnitureUnitIds, List<int> concreteFurnitureComponentIds)
        {
            return await GetAll()
                .Where(ent => concreteFurnitureUnitIds.Contains(ent.ConcreteFurnitureUnitId.Value) || concreteFurnitureComponentIds.Contains(ent.ConcreteFurnitureComponentId.Value))
                .ToListAsync();
        }
    }
}

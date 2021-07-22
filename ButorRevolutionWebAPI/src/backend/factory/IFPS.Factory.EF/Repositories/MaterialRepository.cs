using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Factory.EF.Repositories
{
    public class MaterialRepository : EFCoreRepositoryBase<IFPSFactoryContext, Material, Guid>, IMaterialRepository
    {
        public MaterialRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Material, object>>> DefaultIncludes => new List<Expression<Func<Material, object>>> { };
    }
}

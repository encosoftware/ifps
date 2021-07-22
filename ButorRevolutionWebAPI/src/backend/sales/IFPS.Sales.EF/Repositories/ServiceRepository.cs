using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class ServiceRepository : EFCoreRepositoryBase<IFPSSalesContext, Service>, IServiceRepository
    {
        public ServiceRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Service, object>>> DefaultIncludes => new List<Expression<Func<Service, object>>>
        { };
    }
}

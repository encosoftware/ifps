using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class CompanyTypeRepository : EFCoreRepositoryBase<IFPSSalesContext, CompanyType>, ICompanyTypeRepository
    {
        public CompanyTypeRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<CompanyType, object>>> DefaultIncludes => new List<Expression<Func<CompanyType, object>>>
        {
            p => ((CompanyType)p.Translations),
        };
    }
}

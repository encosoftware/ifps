using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class CompanyTypeRepository : EFCoreRepositoryBase<IFPSFactoryContext, CompanyType>, ICompanyTypeRepository
    {
        public CompanyTypeRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<CompanyType, object>>> DefaultIncludes => new List<Expression<Func<CompanyType, object>>>
        {
            p => ((CompanyType)p.Translations),
        };
    }
}
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class CountryRepository : EFCoreRepositoryBase<IFPSSalesContext, Country>, ICountryRepository
    {
        public CountryRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Country, object>>> DefaultIncludes => new List<Expression<Func<Country, object>>>
        {
            p => ((Country)p.Translations),
        };
    }
}

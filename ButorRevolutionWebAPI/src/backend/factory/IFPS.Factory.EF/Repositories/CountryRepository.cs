using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class CountryRepository : EFCoreRepositoryBase<IFPSFactoryContext, Country>, ICountryRepository
    {
        public CountryRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<Country, object>>> DefaultIncludes => new List<Expression<Func<Country, object>>>
        {
            p => ((Country)p.Translations),
        };
    }
}

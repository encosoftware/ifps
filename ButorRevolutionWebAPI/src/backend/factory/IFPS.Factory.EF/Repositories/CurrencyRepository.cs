using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class CurrencyRepository : EFCoreRepositoryBase<IFPSFactoryContext, Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IFPSFactoryContext context) : base(context)
        {
        }

        protected override List<Expression<Func<Currency, object>>> DefaultIncludes => new List<Expression<Func<Currency, object>>>
        {
        };
    }
}
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class LanguageRepository : EFCoreRepositoryBase<IFPSSalesContext, Language>, ILanguageRepository
    {
        public LanguageRepository(IFPSSalesContext context) : base(context)
        {
        }

        protected override List<Expression<Func<Language, object>>> DefaultIncludes => new List<Expression<Func<Language, object>>>
        {
            p => p.Translations
        };
    }
}

using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class LanguageRepository : EFCoreRepositoryBase<IFPSFactoryContext, Language>, ILanguageRepository
    {
        public LanguageRepository(IFPSFactoryContext context) : base(context)
        {
        }

        protected override List<Expression<Func<Language, object>>> DefaultIncludes => new List<Expression<Func<Language, object>>>
        {
            p => p.Translations
        };
    }
}
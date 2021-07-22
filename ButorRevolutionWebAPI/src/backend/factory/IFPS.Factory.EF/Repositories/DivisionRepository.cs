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
    public class DivisionRepository : EFCoreRepositoryBase<IFPSFactoryContext, Division>, IDivisionRepository
    {
        public DivisionRepository(IFPSFactoryContext context) : base(context)
        {
        }

        protected override List<Expression<Func<Division, object>>> DefaultIncludes => new List<Expression<Func<Division, object>>>
        {
            p => p.Translations
        };

        public Task<List<Division>> GetAllWithClaimsTranslationAsync()
        {
            return GetAllIncluding(ent => ent.Translations, ent => ent.Claims)
                .OrderBy(d => d.DivisionType)
                .ToListAsync();
        }
    }
}
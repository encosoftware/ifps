using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class FrequencyTypeRepository : EFCoreRepositoryBase<IFPSFactoryContext, FrequencyType>, IFrequencyTypeRepository
    {
        public FrequencyTypeRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<FrequencyType, object>>> DefaultIncludes => new List<Expression<Func<FrequencyType, object>>>
        {
            ent=>ent.Translations
        };
    }
}

using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class EmailDataRepository : EFCoreRepositoryBase<IFPSFactoryContext, EmailData>, IEmailDataRepository
    {
        public EmailDataRepository(IFPSFactoryContext context) : base(context) { }
        protected override List<Expression<Func<EmailData, object>>> DefaultIncludes => new List<Expression<Func<EmailData, object>>>
        {
            ent=>ent.Translations
        };
    }
}

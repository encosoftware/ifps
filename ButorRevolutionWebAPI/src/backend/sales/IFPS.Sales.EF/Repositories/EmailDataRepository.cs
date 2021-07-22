using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class EmailDataRepository : EFCoreRepositoryBase<IFPSSalesContext, EmailData>, IEmailDataRepository
    {
        public EmailDataRepository(IFPSSalesContext context) : base(context) { }
        protected override List<Expression<Func<EmailData, object>>> DefaultIncludes => new List<Expression<Func<EmailData, object>>>
        {
            ent=>ent.Translations
        };
    }
}

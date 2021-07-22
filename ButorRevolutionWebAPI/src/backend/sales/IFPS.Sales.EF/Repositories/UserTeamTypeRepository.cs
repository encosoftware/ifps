using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories
{
    public class UserTeamTypeRepository : EFCoreRepositoryBase<IFPSSalesContext, UserTeamType>, IUserTeamTypeRepository
    {
        public UserTeamTypeRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<UserTeamType, object>>> DefaultIncludes => new List<Expression<Func<UserTeamType, object>>>
        {
            ent => ent.Translations
        };
    }
}

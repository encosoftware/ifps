using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IFPS.Factory.EF.Repositories
{
    public class RoleRepository : EFCoreRepositoryBase<IFPSFactoryContext, Role>, IRoleRepository
    {
        public RoleRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<Role, object>>> DefaultIncludes => new List<Expression<Func<Role, object>>>
        {
        };

        public Task<Role> GetRole(int id)
        {
            return GetAll()
                .Include(ent => ent.DefaultRoleClaims)
                    .ThenInclude(ent => ent.Claim)
                        .ThenInclude(ent => ent.Division)
                .SingleAsync(ent => ent.Id == id);
        }

        public Task<List<Role>> GetRoles()
        {
            return GetAll()
                .Include(ent => ent.DefaultRoleClaims)
                    .ThenInclude(ent => ent.Claim)
                        .ThenInclude(ent => ent.Division)
                .ToListAsync();
        }
    }
}
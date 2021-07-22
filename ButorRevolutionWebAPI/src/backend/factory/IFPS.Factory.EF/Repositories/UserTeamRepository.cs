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
    public class UserTeamRepository : EFCoreRepositoryBase<IFPSFactoryContext, UserTeam>, IUserTeamRepository
    {
        public UserTeamRepository(IFPSFactoryContext context) : base(context)
        {
        }

        protected override List<Expression<Func<UserTeam, object>>> DefaultIncludes => new List<Expression<Func<UserTeam, object>>>
        {
        };

        public async Task<List<string>> GetTeamsNamesByUserIdAsync(int userId)
        {
            return await context.UserTeams
                .Where(ent => ent.UserId == userId)
                .Select(ent => ent.TechnicalUser.CurrentVersion.Name)
                .ToListAsync();
        }

        public Task<List<UserTeam>> GetTeamsByCompanyIdAsync(int companyId)
        {
            return context.UserTeams
                .Include(ut => ut.TechnicalUser)
                    .ThenInclude(ut => ut.CurrentVersion)
                .Include(ut => ut.User)
                    .ThenInclude(u => u.CurrentVersion)
                .Where(ut => ut.CompanyId == companyId)
                .ToListAsync();
        }
    }
}
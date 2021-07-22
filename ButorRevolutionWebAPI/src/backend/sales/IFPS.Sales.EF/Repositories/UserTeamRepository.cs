using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class UserTeamRepository : EFCoreRepositoryBase<IFPSSalesContext, UserTeam>, IUserTeamRepository
    {
        public UserTeamRepository(IFPSSalesContext context) : base(context)
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
        // TODO ezt nem használjuk sehol
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

        public async Task<List<int>> GetTechnicalUserIdsByUserIdAsync(int userId)
        {
            var result = await context.UserTeams
                .Where(ut => ut.UserId == userId)
                .Select(ut => ut.TechnicalUserId)
                .ToListAsync();

            return result ?? throw new EntityNotFoundException(typeof(UserTeam), userId);
        }
    }
}

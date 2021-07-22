using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IUserTeamRepository : IRepository<UserTeam>
    {
        Task<List<string>> GetTeamsNamesByUserIdAsync(int userId);

        Task<List<UserTeam>> GetTeamsByCompanyIdAsync(int companyId);
    }
}
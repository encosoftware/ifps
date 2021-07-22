using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IUserTeamRepository : IRepository<UserTeam>
    {
        Task<List<string>> GetTeamsNamesByUserIdAsync(int userId);
        Task<List<UserTeam>> GetTeamsByCompanyIdAsync(int companyId);
        Task<List<int>> GetTechnicalUserIdsByUserIdAsync(int userId);
    }
}

using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetRole(int id);
        Task<List<Role>> GetRoles();
    }
}

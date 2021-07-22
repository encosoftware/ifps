using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IDivisionRepository : IRepository<Division>
    {
        Task<List<Division>> GetAllWithClaimsTranslationAsync();
    }
}

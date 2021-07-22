using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IDivisionRepository : IRepository<Division>
    {
        Task<List<Division>> GetAllWithClaimsTranslationAsync();
    }
}
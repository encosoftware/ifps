using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface ISortingStrategyTypeRepository : IRepository<SortingStrategyType>
    {
        Task<SortingStrategyType> GetSortingStrategyTypeById(int id);
    }
}

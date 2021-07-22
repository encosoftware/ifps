using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IStorageRepository : IRepository<Storage>
    {
        Task<Storage> GetStorageWithIncludesAsync(int id);
    }
}

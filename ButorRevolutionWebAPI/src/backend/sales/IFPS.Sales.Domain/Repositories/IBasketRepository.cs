using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IBasketRepository : IRepository<Basket>
    {
        Task<Basket> GetBasketAsync(int id);
    }
}

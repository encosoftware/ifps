using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IOrderStateRepository : IRepository<OrderState>
    {
        Task<List<OrderState>> GetOrderStatuses();
    }
}

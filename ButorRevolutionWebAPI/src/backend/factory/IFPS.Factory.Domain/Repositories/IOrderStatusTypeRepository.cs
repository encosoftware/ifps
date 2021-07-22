using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IOrderStateRepository : IRepository<OrderState>
    {
        Task<List<OrderState>> GetStatusesForOrderSchedulingAsync();
        Task<List<OrderState>> GetStatusesForFinanceAsync();
    }
}

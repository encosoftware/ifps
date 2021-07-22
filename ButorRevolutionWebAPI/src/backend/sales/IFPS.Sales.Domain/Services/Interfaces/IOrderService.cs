using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<int>> GetOrderAvailableContactIds(Guid orderId);
    }
}

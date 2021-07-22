using IFPS.Sales.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface ITicketAppService
    {
        Task<List<TicketListDto>> GetOwnTicketList(int userId);
        Task<List<TicketListDto>> GetTicketList();
        Task<List<TicketByOrderListDto>> GetTicketsByOrderAsync(Guid orderId);
        Task<List<TicketByOrderListDto>> GetCustomerTicketsByOrderAsync(Guid orderId, int customerId);
    }
}

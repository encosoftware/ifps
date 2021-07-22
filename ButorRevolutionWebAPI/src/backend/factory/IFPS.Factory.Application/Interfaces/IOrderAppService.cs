using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Domain.Dbos.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IOrderAppService
    {
        Task<Guid> CreateOrderAsync(OrderCreateDto dto);
        Task<List<OrderNameListDto>> GetOrderNamesAsync();
        Task AddOrderPaymentAsync(Guid orderId, OrderFinanceCreateDto orderFinanceCreateDto, int userId);
        Task<OrderDetailsDto> GetOrderDetailsAsync(Guid id);
        Task<PagedListDto<OrderFinanceListDto>> GetOrdersByCompanyAsync(int companyId, OrderFinanceFilterDto filter);
        Task ExportOrdersResultsAsync(OrderFinanceFilterDto filter);
        Task ReserveOrFreeUpMaterialsForOrderAsync(Guid orderId, bool isReserved, int userId);
        Task<Dictionary<Guid, RequiredMaterialForOrderDbo>> CalculateRequiredAmount(Guid orderId);
    }
}

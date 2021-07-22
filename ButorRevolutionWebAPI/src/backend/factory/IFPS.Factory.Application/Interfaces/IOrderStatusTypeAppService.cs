using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IOrderStateTypeAppService
    {
        Task<List<OrderStateTypeDropdownListDto>> GetOrderStateTypeDropdownListAsync();
        Task<List<OrderStateTypeDropdownListDto>> GetOrderSchedulingOrderStatesAsync();
        Task<List<OrderStateTypeDropdownListDto>> GetFinanceOrderStatesAsync();
    }
}

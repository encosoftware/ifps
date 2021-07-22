using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IOrderSchedulingAppService
    {
        Task<PagedListDto<OrderSchedulingListDto>> OrderSchedulingListAsync(OrderSchedulingFilterDto dto);
        Task<ProductionStatusDetailsDto> GetProductionStatusByOrderIdAsync(Guid orderId);
    }
}

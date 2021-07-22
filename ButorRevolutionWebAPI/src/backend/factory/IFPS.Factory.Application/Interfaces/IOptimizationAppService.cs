using ENCO.DDD.Application.Dto;
using IFPS.Factory.Application.Dto;
using System;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IOptimizationAppService
    {
        Task<PagedListDto<OptimizationListDto>> GetOptimizationsAsync(OptimizationFilterDto optimizationFilterDto);
        Task StartOrdersOptimization(OrdersForOptimizationDto ordersDto, int userId);
        Task<byte[]> GetLayoutZipAsBytesAsync(Guid optimizationId);
        Task<byte[]> GetScheduleZipAsBytesAsync(Guid optimizationid);
    }
}

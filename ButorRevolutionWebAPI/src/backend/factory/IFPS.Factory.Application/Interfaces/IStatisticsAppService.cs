using IFPS.Factory.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IStatisticsAppService
    {
        Task<StockStatisticsDetailsDto> GetStockStatisticsAsync(StockStatisticsFilterDto stockStatisticsFilterDto);
        Task<List<FinanceStatisticsListDto>> GetFinanceStatisticsAsync(FinanceStatisticsFilterDto financeStatisticsFilterDto);
        Task<GetOldestYearDto> GetOldestYearAsync();
    }
}

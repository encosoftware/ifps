using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IStatisticsAppService
    {
        Task<SalesPersonStatisticsDto> GetSalesPersonsListAsync(SalesPersonFilterDto filterDto);
    }
}

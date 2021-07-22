using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : IFPSControllerBase
    {
        private const string OPNAME = "Statistics";

        private readonly IStatisticsAppService statisticsAppService;

        public StatisticsController(IStatisticsAppService statisticsAppService)
        {
            this.statisticsAppService = statisticsAppService;
        }

        // GET stock statistics
        [HttpGet("stocks")]
        [Authorize(Policy = "GetStockStatistics")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<StockStatisticsDetailsDto> GetStockStatistics([FromQuery] StockStatisticsFilterDto stockStatisticsFilterDto)
        {
            return statisticsAppService.GetStockStatisticsAsync(stockStatisticsFilterDto);
        }

        // GET finance statistics
        [HttpGet("finances")]
        [Authorize(Policy = "GetFinanceStatistics")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<FinanceStatisticsListDto>> GetFinanceStatistics([FromQuery] FinanceStatisticsFilterDto financeStatisticsFilterDto)
        {
            return statisticsAppService.GetFinanceStatisticsAsync(financeStatisticsFilterDto);
        }

        // GET oldest year of payment
        [HttpGet("oldest")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<GetOldestYearDto> GetOldestYear()
        {
            return statisticsAppService.GetOldestYearAsync();
        }
    }
}
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : IFPSControllerBase
    {
        private const string OPNAME = "Statistics";
        private IStatisticsAppService statisticsAppService;

        public StatisticsController(IStatisticsAppService statisticsAppService)
        {
            this.statisticsAppService = statisticsAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetStatistics")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<SalesPersonStatisticsDto> GetSalesPersonsList([FromQuery] SalesPersonFilterDto filterDto)
        {
            return statisticsAppService.GetSalesPersonsListAsync(filterDto);
        }
    }
}

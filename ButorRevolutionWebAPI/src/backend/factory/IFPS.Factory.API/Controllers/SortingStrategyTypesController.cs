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
    [Route("api/sortingstrategies")]
    [ApiController]
    public class SortingStrategiesController : IFPSControllerBase
    {
        private const string OPNAME = "SortingStrategies";
        private readonly ISortingStrategyTypesAppService sortingStrategiesService;

        public SortingStrategiesController(ISortingStrategyTypesAppService sortingStrategiesService)
        {
            this.sortingStrategiesService = sortingStrategiesService;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<SortingStrategyTypeListDto>> GetSortingStrategiesForDropdown()
        {
            return sortingStrategiesService.GetSortingStrategyTypesAsync();
        }
    }
}

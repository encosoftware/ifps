using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/sortings")]
    [ApiController]
    public class SortingController : IFPSControllerBase
    {
        private const string OPNAME = "Sortings";
        private readonly ISortingsAppService sortingsAppService;

        public SortingController(ISortingsAppService sortingsAppService)
        {
            this.sortingsAppService = sortingsAppService;
        }

        // Get all sortings
        [HttpGet]
        [Authorize(Policy = "GetSortings")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<SortingListDto>> GetSortings([FromQuery]SortingFilterDto dto)
        {
            return sortingsAppService.GetSortingsAsync(dto);
        }
    }
}

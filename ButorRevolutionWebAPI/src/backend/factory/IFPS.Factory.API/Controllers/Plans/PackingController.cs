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
    [Route("api/packings")]
    [ApiController]
    public class PackingController : IFPSControllerBase
    {
        private const string OPNAME = "Packings";
        private readonly IPackingsAppService packingsAppService;

        public PackingController(IPackingsAppService packingsAppService)
        {
            this.packingsAppService = packingsAppService;
        }

        // Get all packings
        [HttpGet]
        [Authorize(Policy = "GetPackings")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<PackingListDto>> GetPackings([FromQuery]PackingFilterDto dto)
        {
            return packingsAppService.GetPackingsAsync(dto);
        }
    }
}

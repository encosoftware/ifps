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
    [Route("api/edgebanding")]
    [ApiController]
    public class EdgebandingController : IFPSControllerBase
    {
        private const string OPNAME = "Edgebandings";
        private readonly IEdgebandingAppService service;

        public EdgebandingController(IEdgebandingAppService service)
        {
            this.service = service;
        }

        // Get all CNC
        [HttpGet]
        [Authorize(Policy = "GetEdgeBandings")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<EdgebandingListDto>> EdgebandingList([FromQuery]EdgebandingFilterDto dto)
        {
            return service.EdgebandingListAsync(dto);
        }
    }
}

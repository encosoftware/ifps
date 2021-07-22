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
    [Route("api/cnc")]
    [ApiController]
    public class CNCController : IFPSControllerBase
    {
        private const string OPNAME = "CNC";
        private readonly ICncAppService service;

        public CNCController(ICncAppService service)
        {
            this.service = service;
        }

        // Get all CNC
        [HttpGet]
        [Authorize(Policy = "GetCncs")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<CNCListDto>> CNCList([FromQuery]CncFilterDto dto)
        {
            return service.CncListAsync(dto);
        }
    }
}

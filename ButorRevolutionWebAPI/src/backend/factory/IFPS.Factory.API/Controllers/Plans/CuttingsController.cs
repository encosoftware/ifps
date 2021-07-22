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
    [Route("api/cuttings")]
    [ApiController]
    public class CuttingsController : IFPSControllerBase
    {
        private const string OPNAME = "Cuttings";
        private readonly ICuttingsAppService service;

        public CuttingsController(ICuttingsAppService service)
        {
            this.service = service;
        }

        // Get all cuttings
        [HttpGet]
        [Authorize(Policy = "GetCuttings")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<CuttingsListDto>> CuttingsList([FromQuery]CuttingsFilterDto dto)
        {
            return service.CuttingsListAsync(dto);
        }
    }
}

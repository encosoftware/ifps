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
    [Route("api/assemblies")]
    [ApiController]
    public class AssemblyController : IFPSControllerBase
    {
        private const string OPNAME = "Assemblies";
        private readonly IAssemblyAppService service;

        public AssemblyController(IAssemblyAppService service)
        {
            this.service = service;
        }

        // Get all Assembly
        [HttpGet]
        [Authorize(Policy = "GetAssemblies")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<AssemblyListDto>> AssemblyList([FromQuery]AssemblyFilterDto dto)
        {
            return service.AssemblyListAsync(dto);
        }
    }
}

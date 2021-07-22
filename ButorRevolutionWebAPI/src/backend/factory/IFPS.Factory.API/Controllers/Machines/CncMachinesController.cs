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
    [Route("api/cncmachines")]
    [ApiController]
    public class CncMachinesController : IFPSControllerBase
    {
        private const string OPNAME = "CncMachines";
        private readonly ICncMachineAppService cncMachineAppService;

        public CncMachinesController(ICncMachineAppService cncMachineAppService)
        {
            this.cncMachineAppService = cncMachineAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<CncMachineListDto>> GetCncMachines([FromQuery]CncMachineFilterDto dto)
        {
            return cncMachineAppService.GetCncMachinesAsync(dto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<CncMachineDetailsDto> GetCncMachine(int id)
        {
            return cncMachineAppService.GetCncMachineByIdAsync(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateCncMachine(int id, [FromBody]CncMachineUpdateDto dto)
        {
            return cncMachineAppService.UpdateCncMachineAsync(id, dto);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateCncMachine([FromBody]CncMachineCreateDto dto)
        {
            return cncMachineAppService.CreateCncMachineAsync(dto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteCncMachine(int id)
        {
            return cncMachineAppService.DeleteCncMachineAsync(id);
        }
    }
}

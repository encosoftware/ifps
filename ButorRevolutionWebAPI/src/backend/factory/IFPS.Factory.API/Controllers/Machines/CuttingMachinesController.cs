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
    [Route("api/cuttingmachines")]
    [ApiController]
    public class CuttingMachineController : IFPSControllerBase
    {
        private const string OPNAME = "CuttingMachines";
        private readonly ICuttingMachineAppService cuttingMachineAppService;

        public CuttingMachineController(ICuttingMachineAppService cuttingMachineAppService)
        {
            this.cuttingMachineAppService = cuttingMachineAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<CuttingMachineListDto>> GetCuttingMachines([FromQuery]CuttingMachineFilterDto dto)
        {
            return cuttingMachineAppService.GetCuttingMachinesAsync(dto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<CuttingMachineDetailsDto> GetCuttingMachine(int id)
        {
            return cuttingMachineAppService.GetCuttingMachineByIdAsync(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateCuttingMachine(int id, [FromBody]CuttingMachineUpdateDto dto)
        {
            return cuttingMachineAppService.UpdateCuttingMachineAsync(id, dto);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateCuttingMachine([FromBody]CuttingMachineCreateDto dto)
        {
            return cuttingMachineAppService.CreateCuttingMachineAsync(dto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteCuttingMachine(int id)
        {
            return cuttingMachineAppService.DeleteCuttingMachineAsync(id);
        }
    }
}

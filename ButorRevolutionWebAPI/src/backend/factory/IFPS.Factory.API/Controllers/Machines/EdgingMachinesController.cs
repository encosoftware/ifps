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
    [Route("api/edgingmachines")]
    [ApiController]
    public class EdgingMachinesController : IFPSControllerBase
    {
        private const string OPNAME = "Edgingmachines";
        private readonly IEdgingMachineAppService appService;

        public EdgingMachinesController(IEdgingMachineAppService appService)
        {
            this.appService = appService;
        }

        [HttpGet]
        [Authorize(Policy = "GetMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<EdgingMachineListDto>> GetEdgingMachines([FromQuery]EdgingMachineFilterDto dto)
        {
            return appService.GetEdgingMachinesAsync(dto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<EdgingMachineDetailsDto> GetEdgingMachine(int id)
        {
            return appService.GetEdgingMachineByIdAsync(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateEdgingMachine(int id, [FromBody]EdgingMachineUpdateDto dto)
        {
            return appService.UpdateEdgingMachineAsync(id, dto);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateEdgingMachine([FromBody]EdgingMachineCreateDto dto)
        {
            return appService.CreateEdgingMachineAsync(dto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMachines")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteEdgingMachine(int id)
        {
            return appService.DeleteEdgingMachineAsync(id);
        }
    }
}

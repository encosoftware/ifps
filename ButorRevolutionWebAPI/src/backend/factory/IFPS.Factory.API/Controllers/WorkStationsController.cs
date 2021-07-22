using ENCO.DDD.Application.Dto;
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
    [Route("api/workstations")]
    [ApiController]
    public class WorkStationsController : IFPSControllerBase
    {
        private const string OPNAME = "WorkStations";
        private readonly IWorkStationsAppService workStationsAppService;

        public WorkStationsController(IWorkStationsAppService workStationsAppService)
        {
            this.workStationsAppService = workStationsAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetWorkstations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<WorkStationListDto>> GetWorkStations([FromQuery] WorkStationFilterDto filterDto)
        {
            return workStationsAppService.GetWorkStationsAsync(filterDto);
        }

        [HttpGet("{workStationId}")]
        [Authorize(Policy = "GetWorkstations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<WorkStationDetailsDto> GetWorkStation(int workStationId)
        {
            return workStationsAppService.GetWorkStationAsync(workStationId);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateWorkstations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateWorkStation([FromBody] WorkStationCreateDto createDto)
        {
            return workStationsAppService.CreateWorkStationAsync(createDto);
        }

        [HttpPut("{workStationId}")]
        [Authorize(Policy = "UpdateWorkstations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateWorkStation(int workStationId, [FromBody] WorkStationUpdateDto updateDto)
        {
            return workStationsAppService.UpdateWorkStationAsync(workStationId, updateDto);
        }

        [HttpDelete("{workStationId}")]
        [Authorize(Policy = "DeleteWorkstations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteWorkStation(int workStationId)
        {
            return workStationsAppService.DeleteWorkStationAsync(workStationId);
        }

        // activate or deactivate WorkStation
        [HttpPut("{workStationId}/activate")]
        [Authorize(Policy = "UpdateWorkstations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task SetAvailabilityOfWorkStation(int workStationId)
        {
            return workStationsAppService.SetAvailabilityOfWorkStationAsync(workStationId);
        }

        [HttpPut("{workStationId}/cameras")]
        [Authorize(Policy = "UpdateWorkstations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task AddCameras(int workStationId, [FromBody] WorkStationCameraCreateDto workStationCameraCreateDto)
        {
            return workStationsAppService.AddCamerasAsync(workStationId, workStationCameraCreateDto);
        }

        [HttpGet("{workStationId}/cameras")]
        [Authorize(Policy = "GetWorkstations")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<WorkStationCameraDetailsDto> GetCameras(int workStationId)
        {
            return workStationsAppService.GetCamerasAsync(workStationId);
        }
    }
}
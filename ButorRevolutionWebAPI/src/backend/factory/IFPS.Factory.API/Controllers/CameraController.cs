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
    [Route("api/cameras")]
    [ApiController]
    public class CameraController : IFPSControllerBase
    {
        private const string OPNAME = "Cameras";
        private readonly ICameraAppService cameraAppService;

        public CameraController(ICameraAppService cameraAppService)
        {
            this.cameraAppService = cameraAppService;
        }

        [HttpPost]
        [Authorize(Policy = "UpdateCameras")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateCamera([FromBody]CameraCreateDto cameraCreateDto)
        {
            return cameraAppService.CreateCameraAsync(cameraCreateDto);
        }

        [HttpGet]
        [Authorize(Policy = "GetCameras")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<CameraListDto>> GetCameras([FromQuery]CameraFilterDto cameraFilterDto)
        {
            return cameraAppService.GetCamerasAsync(cameraFilterDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetCameras")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<CameraDetailsDto> GetCamera(int id)
        {
            return cameraAppService.GetCameraAsync(id);
        }        

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateCameras")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateCamera(int id, [FromBody]CameraUpdateDto cameraUpdateDto)
        {
            return cameraAppService.UpdateCameraAsync(id, cameraUpdateDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteCameras")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteCamera(int id)
        {
            return cameraAppService.DeleteCameraAsync(id);
        }

        [HttpGet("names/{workStationId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<CameraNameListDto>> GetCameraNameList(int workStationId)
        {
            return cameraAppService.GetCameraNameListAsync(workStationId);
        }
    }
}

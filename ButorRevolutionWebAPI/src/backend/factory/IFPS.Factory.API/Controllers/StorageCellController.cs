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
    [Route("api/storagecells")]
    [ApiController]
    public class StorageCellController : IFPSControllerBase
    {
        private const string OPNAME = "StorageCells";
        private readonly IStorageCellAppService service;

        public StorageCellController(IStorageCellAppService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(Policy = "UpdateStorageCells")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateStorageCellAsync([FromBody]StorageCellCreateDto storageCellCreateDto)
        {
            return service.CreateStorageCellAsync(storageCellCreateDto);
        }

        [HttpGet]
        [Authorize(Policy = "GetStorageCells")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<StorageCellListDto>> GetStorageCellsAsync([FromQuery]StorageCellFilterDto storageCellFilterDto)
        {
            return service.GetStorageCellsAsync(storageCellFilterDto);
        }

        [HttpGet("download")]
        [Authorize(Policy = "GetStorageCells")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task DownloadFilteredStorageResultsAsync([FromQuery]StorageCellFilterDto storageCellFilterDto)
        {
            await service.DownloadStorageCellResultsAsync(storageCellFilterDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetStorageCells")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<StorageCellDetailsDto> GetStorageCellAsync(int id)
        {
            return service.GetStorageCellAsync(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateStorageCells")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateStorageCellAsync(int id, [FromBody] StorageCellUpdateDto storageCellUpdateDto)
        {
            return service.UpdateStorageCellAsync(id, storageCellUpdateDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteStorageCells")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteStorageCellAsync(int id)
        {
            return service.DeleteStorageCellAsync(id);
        }

        // Storage Cell list for dropdown
        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<StorageCellDropdownListDto>> StorageCellDropdownList()
        {
            return service.ListDropDownStorageCellsAsync();
        }
    }
}

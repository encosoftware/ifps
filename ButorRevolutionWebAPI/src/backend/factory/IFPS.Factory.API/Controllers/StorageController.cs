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
    [Route("api/storages")]
    [ApiController]
    public class StorageController : IFPSControllerBase
    {
        private const string OPNAME = "Storages";
        private readonly IStorageAppService service;

        public StorageController(IStorageAppService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Authorize(Policy = "UpdateStorages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateStorageAsync([FromBody]StorageCreateDto storageCreateDto)
        {
            return service.CreateStorageAsync(storageCreateDto);
        }

        [HttpGet]
        [Authorize(Policy = "GetStorages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<StorageListDto>> GetStoragesAsync([FromQuery]StorageFilterDto storageFilterDto)
        {
            return service.GetStoragesAsync(storageFilterDto);
        }

        [HttpGet("download")]
        [Authorize(Policy = "GetStorageCells")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task DownloadFilteredStorageResultsAsync([FromQuery]StorageFilterDto storageFilterDto)
        {
            await service.DownloadStorageResultsAsync(storageFilterDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetStorages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<StorageDetailsDto> GetStorageAsync(int id)
        {
            return service.GetStorageAsync(id);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateStorages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateStorageAsync(int id, [FromBody] StorageUpdateDto storageUpdateDto)
        {
            return service.UpdateStorageAsync(id, storageUpdateDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteStorages")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteStorageAsync(int id)
        {
            return service.DeleteStorageAsync(id);
        }

        [HttpGet("names")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<StorageNameListDto>> GetStorageNameListAsync()
        {
            return service.GetStorageNameListAsync();
        }
    }
}

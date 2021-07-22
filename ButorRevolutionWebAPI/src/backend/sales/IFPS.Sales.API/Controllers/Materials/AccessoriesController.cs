using ENCO.DDD.Application.Dto;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/accessories")]
    [ApiController]
    public class AccessoriesController : IFPSControllerBase
    {
        private const string OPNAME = "Accessories";

        private readonly IAccessoryMaterialAppService accessoryMaterialAppService;

        public AccessoriesController(
            IAccessoryMaterialAppService accessoryMaterialAppService)
        {
            this.accessoryMaterialAppService = accessoryMaterialAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<AccessoryMaterialListDto>> GetAccessoryMaterials([FromQuery]AccessoryMaterialFilterDto filter)
        {
            return accessoryMaterialAppService.GetAccessoryMaterialsAsync(filter);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<Guid> CreateAccessoryMaterial([FromBody] AccessoryMaterialCreateDto model)
        {
            return await accessoryMaterialAppService.CreateAccessoryMaterialAsync(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task PutAccessoryMaterial(Guid id, [FromBody] AccessoryMaterialUpdateDto model)
        {
            return accessoryMaterialAppService.UpdateAccessoryMaterialAsync(id, model);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<AccessoryMaterialDetailsDto> GetAccessoryMaterialById(Guid id)
        {
            return accessoryMaterialAppService.GetAccessoryMaterialAsync(id);
        }

        [HttpGet("code")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<AccessoryMaterialCodesDto>> GetAccessoryMaterialCodes()
        {
            return accessoryMaterialAppService.GetAccessoryMaterialCodesAsync();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteAccessoryMaterial(Guid id)
        {
            return accessoryMaterialAppService.DeleteMaterialAsync(id);
        }
    }
}
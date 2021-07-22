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
    [Route("api/appliances")]
    [ApiController]
    public class AppliancesController : IFPSControllerBase
    {
        private const string OPNAME = "Appliances";

        private readonly IApplianceMaterialAppService applianceMaterialAppService;

        public AppliancesController(
            IApplianceMaterialAppService applianceMaterialAppService)
        {
            this.applianceMaterialAppService = applianceMaterialAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<ApplianceMaterialListDto>> GetAccessoryMaterials([FromQuery]ApplianceMaterialFilterDto filter)
        {
            return applianceMaterialAppService.GetApplianceMaterialsAsync(filter);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> CreateApplianceMaterial([FromBody] ApplianceMaterialCreateDto model)
        {
            return applianceMaterialAppService.CreateApplianceMaterialAsync(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task PutApplianceMaterial(Guid id, [FromBody] ApplianceMaterialUpdateDto model)
        {
            return applianceMaterialAppService.UpdateApplianceMaterialAsync(id, model);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<ApplianceMaterialDetailsDto> GetApplianceMaterialById(Guid id)
        {
            return applianceMaterialAppService.GetApplianceMaterialAsync(id);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteApplianceMaterial(Guid id)
        {
            return applianceMaterialAppService.DeleteMaterialAsync(id);
        }

        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<ApplianceMaterialsListForDropdownDto>> GetApplianceMaterilasForDropdown()
        {
            return applianceMaterialAppService.GetApplianceMaterialsForDropdownAsync();
        }
    }
}
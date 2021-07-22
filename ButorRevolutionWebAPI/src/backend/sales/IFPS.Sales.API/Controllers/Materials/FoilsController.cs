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
    [Route("api/foils")]
    [ApiController]
    public class FoilsController : IFPSControllerBase
    {
        private const string OPNAME = "Foils";

        private readonly IFoilMaterialAppService foilMaterialAppService;

        public FoilsController(
            IFoilMaterialAppService foilMaterialAppService)
        {
            this.foilMaterialAppService = foilMaterialAppService;
        }

        [HttpGet]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<FoilMaterialListDto>> GetAccessoryMaterials([FromQuery]FoilMaterialFilterDto filter)
        {
            return foilMaterialAppService.GetFoilMaterialsAsync(filter);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> CreateFoilMaterial([FromBody] FoilMaterialCreateDto model)
        {
            return foilMaterialAppService.CreateFoilMaterialAsync(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task PutFoilMaterial(Guid id, [FromBody] FoilMaterialUpdateDto model)
        {
            return foilMaterialAppService.UpdateFoilMaterialAsync(id, model);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<FoilMaterialDetailsDto> GetFoilMaterialById(Guid id)
        {
            return foilMaterialAppService.GetFoilMaterialAsync(id);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteMaterials")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteFoilMaterial(Guid id)
        {
            return foilMaterialAppService.DeleteMaterialAsync(id);
        }

        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<FoilsForDropdownDto>> GetFoilsForDropdown()
        {
            return foilMaterialAppService.GetFoilsForDropdownAsync();
        }
    }
}
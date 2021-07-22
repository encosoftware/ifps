using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/furniturecomponents")]
    [ApiController]
    public class FurnitureComponentsController : IFPSControllerBase
    {
        private const string OPNAME = "FurnitureComponents";

        private readonly IFurnitureComponentAppService furnitureComponentAppService;

        public FurnitureComponentsController(
            IFurnitureComponentAppService furnitureComponentAppService)
        {
            this.furnitureComponentAppService = furnitureComponentAppService;
        }

        //GET furniture component by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<FurnitureComponentDetailsDto> GetFurnitureComponentById(Guid id)
        {
            return await furnitureComponentAppService.GetFurnitureComponentDetailsAsync(id);
        }

        // POST furniture component
        [HttpPost]
        [Authorize(Policy = "UpdateFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<Guid> PostFurnitureComponent([FromBody] FurnitureComponentCreateDto furnitureComponentCreateDto)
        {
            return await furnitureComponentAppService.CreateFurnitureComponentAsync(furnitureComponentCreateDto);
        }

        // PUT furniture component
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task UpdateFurnitureComponent(Guid id, [FromBody]FurnitureComponentUpdateDto furnitureComponentUpdateDto)
        {
            await furnitureComponentAppService.UpdateFurnitureComponentAsync(id, furnitureComponentUpdateDto);
        }

        // DELETE furniture component
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task DeleteFurnitureComponent(Guid id)
        {
            await furnitureComponentAppService.DeleteFurnitureComponentAsync(id);
        }
    }
}
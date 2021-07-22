using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ENCO.DDD.Application.Dto;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/furnitureunits")]
    [ApiController]
    public class FurnitureUnitsController : IFPSControllerBase
    {
        private const string OPNAME = "FurnitureUnits";

        private readonly IFurnitureUnitAppService furnitureUnitAppService;

        public FurnitureUnitsController(
            IFurnitureUnitAppService furnitureUnitAppService)
        {
            this.furnitureUnitAppService = furnitureUnitAppService;
        }

        // GET furniture unit list
        [HttpGet]
        [Authorize(Policy = "GetFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<FurnitureUnitListDto>> GetFurnitureUnits([FromQuery]FurnitureUnitFilterDto filter)
        {
            return furnitureUnitAppService.GetFurnitureUnitsAsync(filter);
        }

        //GET furniture unit by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<FurnitureUnitDetailsDto> GetFurnitureUnitById(Guid id)
        {
            return furnitureUnitAppService.GetFurnitureUnitDetailsAsync(id);
        }

        //GET furniture unit by id
        [HttpGet("{id}/wfus")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<FurnitureUnitForWFUDto> GetFurnitureUnitForWFU(Guid id)
        {
            return furnitureUnitAppService.GetFurnitureUnitForWFUAsync(id);
        }

        // POST furniture unit
        [HttpPost]
        [Authorize(Policy = "UpdateFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> PostFurnitureUnit([FromBody] FurnitureUnitCreateDto furnitureUnitCreateDto)
        {
            return furnitureUnitAppService.CreateFurnitureUnitAsync(furnitureUnitCreateDto);
        }

        // PUT furniture unit
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateFurnitureUnit(Guid id, [FromBody]FurnitureUnitUpdateDto furnitureUnitUpdateDto)
        {
            return furnitureUnitAppService.UpdateFurnitureUnitAsync(id, furnitureUnitUpdateDto);
        }

        // DELETE furniture unit
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteFurnitureUnit(Guid id)
        {
            return furnitureUnitAppService.DeleteFurnitureUnitAsync(id);
        }

        // GET top cabinetfurniture units for dropdown
        [HttpGet("top/dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<FurnitureUnitsForDropdownDto>> GetTopFurnitureUnits()
        {
            return furnitureUnitAppService.GetTopCabinetFurnitureUnitsAsync();
        }

        // GET base cabinet furniture units for dropdown
        [HttpGet("base/dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<FurnitureUnitsForDropdownDto>> GetBaseFurnitureUnits()
        {
            return furnitureUnitAppService.GetBaseCabinetFurnitureUnitsAsync();
        }

        // GET tall cabinet furniture units for dropdown
        [HttpGet("tall/dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<FurnitureUnitsForDropdownDto>> GetTallFurnitureUnits()
        {
            return furnitureUnitAppService.GetTallCabinetFurnitureUnitsAsync();
        }

        // GET furnitureunits when create webshopfurnitureunit
        [HttpGet("webshopfurnitureunits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<FurnitureUnitListByWebshopFurnitureUnitDto>> GetFurnitureUnitsByWebshopFurnitureUnit([FromQuery]FurnitureUnitCodeFilterDto filterDto)
        {
            return furnitureUnitAppService.GetFurnitureUnitsByWebshopFurnitureUnitAsync(filterDto);
        }

        // GET furnitureunit by id when create webshopfurnitureunit
        [HttpGet("{furnitureUnitId}/webshopfurnitureunits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<FurnitureUnitDetailsByWebshopFurnitureUnitDto> GetFurnitureUnitDetailsByWebshopFurnitureUnit(Guid furnitureUnitId)
        {
            return furnitureUnitAppService.GetFurnitureUnitDetailsByWebshopFurnitureUnitAsync(furnitureUnitId);
        }

        [HttpPost("fileSeed")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task CreateFurnitureUnitFromFile(FurnitureUnitFileSeedDto furnitureUnitSeed)
        {
            // approved by Spanish Inquisition
            var containerInfo = new DirectoryInfo(Path.Combine("AppData", furnitureUnitSeed.ContainerName));
            foreach (var unitFolder in containerInfo.GetDirectories())
            {
                string unitFolderName = unitFolder.Name;           
                string fileName = Path.Combine(unitFolderName, unitFolderName + ".csv");
                await furnitureUnitAppService.CreateFurnitureUnitFromFileAsync(furnitureUnitSeed.ContainerName, fileName);
            }

            return;
        }
    }
}
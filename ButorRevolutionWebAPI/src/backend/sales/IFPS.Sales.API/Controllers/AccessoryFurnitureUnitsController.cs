using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/accessoryfurnitureunits")]
    [ApiController]
    public class AccessoryFurnitureUnitsController : IFPSControllerBase
    {
        private const string OPNAME = "Accessoryfurnitureunits";

        private readonly IAccessoryFurnitureUnitAppService accessoryFurnitureUnitAppService;

        public AccessoryFurnitureUnitsController(
            IAccessoryFurnitureUnitAppService accessoryFurnitureUnitAppService)
        {
            this.accessoryFurnitureUnitAppService = accessoryFurnitureUnitAppService;
        }

        //GET accessory furniture unit by id
        [HttpGet("{id}")]
        [Authorize(Policy = "GetFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<AccessoryFurnitureUnitDetailsDto> GetAccessoryFurnitureUnitById(int id)
        {
            return await accessoryFurnitureUnitAppService.GetAccessoryFurnitureUnitDetailsAsync(id);
        }

        // POST accessory furniture unit
        [HttpPost]
        [Authorize(Policy = "UpdateFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<int> CreateAccessoryFurnitureUnit([FromBody] AccessoryFurnitureUnitCreateDto accessoryComponentCreateDto)
        {
            return await accessoryFurnitureUnitAppService.CreateAccessoryFurnitureUnitAsync(accessoryComponentCreateDto);
        }

        // PUT accessory furniture unit
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task UpdateAccessoryFurnitureUnit(int id, [FromBody]AccessoryFurnitureUnitUpdateDto accessoryComponentUpdateDto)
        {
            await accessoryFurnitureUnitAppService.UpdateAccessoryFurnitureUnitAsync(id, accessoryComponentUpdateDto);
        }

        // DELETE accessory furniture unit
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task DeleteFurnitureUnit(int id)
        {
            await accessoryFurnitureUnitAppService.DeleteAccessoryFurnitureUnitAsync(id);
        }
    }
}
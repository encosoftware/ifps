using ENCO.DDD.Application.Dto;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/webshopfurnitureunits")]
    [ApiController]
    public class WebshopFurnitureUnitController : IFPSControllerBase
    {
        private const string OPNAME = "WebshopFurnitureUnits";
        private readonly IWebshopFurnitureUnitsAppService webshopFurnitureUnitAppService;

        public WebshopFurnitureUnitController(IWebshopFurnitureUnitsAppService webshopFurnitureUnitAppService)
        {
            this.webshopFurnitureUnitAppService = webshopFurnitureUnitAppService;
        }

        [HttpPost]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateWebshopFurnitureUnit([FromBody] WebshopFurnitureUnitCreateDto createDto)
        {
            return webshopFurnitureUnitAppService.CreateWebshopFurnitureUnitAsync(createDto);
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<WebshopFurnitureUnitListDto>> GetWebshopFurnitureUnits([FromQuery] WebshopFurnitureUnitFilterDto filterDto)
        {
            return webshopFurnitureUnitAppService.GetWebshopFurnitureUnitsAsync(filterDto);
        }

        [HttpGet("{wfuId}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<WebshopFurnitureUnitDetailsDto> GetWebshopFurnitureUnitById(int wfuId)
        {
            return webshopFurnitureUnitAppService.GetWebshopFurnitureUnitByIdAsync(wfuId);
        }

        [HttpPut("{wfuId}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateWebshopFurnitureUnit(int wfuId, [FromBody] WebshopFurnitureUnitUpdateDto updateDto)
        {
            return webshopFurnitureUnitAppService.UpdateWebshopFurnitureUnitAsync(wfuId, updateDto);
        }

        [HttpDelete("{wfuId}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteWebshopFurnitureUnit(int wfuId)
        {
            return webshopFurnitureUnitAppService.DeleteWebshopFurnitureUnitAsync(wfuId);
        }

        [HttpGet("{subcategoryId}/subcategory")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<WebshopFurnitureUnitListByWebshopCategoryDto>> GetWebshopFurnitureUnitsByWebshopCategory(int subcategoryId, [FromQuery]WebshopFurnitureUnitFilterByWebshopCategoryDto filterDto)
        {
            return webshopFurnitureUnitAppService.GetWebshopFurnitureUnitsByWebshopCategoryAsync(subcategoryId, filterDto);
        }

        [HttpGet("{webshopFurnitureUnitId}/webshop")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<WebshopFurnitureUnitByWebshopDetailsDto> GetFurnitureUnitByWebshopById(int webshopFurnitureUnitId)
        {
            return webshopFurnitureUnitAppService.GetWebshopFurnitureUnitByWebshopByIdAsync(webshopFurnitureUnitId);
        }

        [HttpGet("webshop/search")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<WebshopFurnitureUnitListByWebshopCategoryDto>> SearchWebshopFurnitureUnitInWebshop([FromQuery]WebshopFurnitureUnitFilterDto filterDto)
        {
            return webshopFurnitureUnitAppService.SearchWebshopFurnitureUnitInWebshopAsync(filterDto);
        }

        [HttpPost("/webshop/maximumprice")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PriceListDto> GetMaximumPriceFromWFUList([FromBody] WebshopFurnitureUnitCategoryIdsDto dto)
        {
            return webshopFurnitureUnitAppService.GetMaximumPriceFromWFUListAsync(dto);
        }

        [HttpGet("dropdown")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<WebshopFurnitureUnitDropdownListDto>> GetWebshopFurnitureUnitsForDropdown()
        {
            return await webshopFurnitureUnitAppService.GetWebshopFurnitureUnitsForDropdownAsync();
        }

        [HttpPost("recommendation")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<WebshopFurnitureUnitListByWebshopCategoryDto>> GetRecommendedFurnitureUnits([FromBody]WebshopFurnitureUnitInBasketIdsDto dto)
        {
            return await webshopFurnitureUnitAppService.GetRecommendedFurnitureUnitsAsync(dto);
        }

        //TODO: create a job from this
        [HttpPost("updaterecrules")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<(string ContainerName, string Filename)> UpdateRecommendationRules()
        {
            return await webshopFurnitureUnitAppService.UpdateRecommendationRulesAsync();
        }
    }
}

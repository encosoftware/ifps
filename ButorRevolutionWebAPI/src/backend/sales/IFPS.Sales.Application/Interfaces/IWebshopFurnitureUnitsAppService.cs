using ENCO.DDD.Application.Dto;
using IFPS.Sales.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IWebshopFurnitureUnitsAppService
    {
        Task<int> CreateWebshopFurnitureUnitAsync([FromBody] WebshopFurnitureUnitCreateDto createDto);
        Task<PagedListDto<WebshopFurnitureUnitListDto>> GetWebshopFurnitureUnitsAsync(WebshopFurnitureUnitFilterDto filterDto);
        Task<WebshopFurnitureUnitDetailsDto> GetWebshopFurnitureUnitByIdAsync(int wfuId);
        Task UpdateWebshopFurnitureUnitAsync(int wfuId, WebshopFurnitureUnitUpdateDto updateDto);
        Task DeleteWebshopFurnitureUnitAsync(int wfuId);
        Task<PagedListDto<WebshopFurnitureUnitListByWebshopCategoryDto>> GetWebshopFurnitureUnitsByWebshopCategoryAsync(int subcategoryId, [FromQuery]WebshopFurnitureUnitFilterByWebshopCategoryDto filterDto);
        Task<WebshopFurnitureUnitByWebshopDetailsDto> GetWebshopFurnitureUnitByWebshopByIdAsync(int webshopFurnitureUnitId);
        Task<PagedListDto<WebshopFurnitureUnitListByWebshopCategoryDto>> SearchWebshopFurnitureUnitInWebshopAsync([FromQuery]WebshopFurnitureUnitFilterDto filterDto);
        Task<PriceListDto> GetMaximumPriceFromWFUListAsync(WebshopFurnitureUnitCategoryIdsDto dto);
        Task<List<WebshopFurnitureUnitDropdownListDto>> GetWebshopFurnitureUnitsForDropdownAsync();
        Task<List<WebshopFurnitureUnitListByWebshopCategoryDto>> GetRecommendedFurnitureUnitsAsync(WebshopFurnitureUnitInBasketIdsDto dto);
        Task<(string ContainerName, string Filename)> UpdateRecommendationRulesAsync();
    }
}

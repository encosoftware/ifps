using IFPS.Sales.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface ICustomerAppService
    {
        Task UpdateProductRecommendationsAsync(int customerId, FurnitureRecommendationDto dto);
        Task<List<WebshopFurnitureUnitListByWebshopCategoryDto>> GetProductRecommendationsAsync(int customerId);
    }
}

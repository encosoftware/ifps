using IFPS.Sales.Application.Dto;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Interfaces
{
    public interface IBasketAppService
    {
        Task<BasketDetailsDto> GetBasketDetailsAsync(int id);
        Task<int> CreateBasketAsync(BasketCreateDto basketCreateDto);
        Task UpdateBasketAsync(int id, BasketUpdateDto basketUpdateDto, bool isUpdate);
        Task PurchaseBasketAsync(int id, BasketPurchaseDto basketPurchaseDto);
        Task DeleteBasketItemAsync(int basketId, Guid furnitureUnitId);
        Task<int> SynchronizeBasketsAsync(int basketId, int otherBasketId);
    }
}

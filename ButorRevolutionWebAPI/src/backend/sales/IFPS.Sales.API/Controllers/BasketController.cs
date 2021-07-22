using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/baskets")]
    [ApiController]
    public class BasketController : IFPSControllerBase
    {
        private const string OPNAME = "Baskets";

        private readonly IBasketAppService basketAppService;

        public BasketController(IBasketAppService basketAppService)
        {
            this.basketAppService = basketAppService;
        }

        // GET basket by id
        [HttpGet("{id}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<BasketDetailsDto> GetBasketDetails(int id)
        {
            return basketAppService.GetBasketDetailsAsync(id);
        }

        // POST basket
        [HttpPost]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> CreateBasket([FromBody]BasketCreateDto basketCreateDto)
        {
            return basketAppService.CreateBasketAsync(basketCreateDto);
        }

        // PUT basket by id
        [HttpPut("{id}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateBasket(int id, [FromBody]BasketUpdateDto basketUpdateDto, bool isUpdate)
        {
            return basketAppService.UpdateBasketAsync(id, basketUpdateDto, isUpdate);
        }

        // PUT basket by id
        [HttpPut("{id}/purchase")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task PurchaseBasket(int id, [FromBody]BasketPurchaseDto basketPurchaseDto)
        {
            return basketAppService.PurchaseBasketAsync(id, basketPurchaseDto);
        }

        // DELETE basket item by id
        [HttpDelete("{basketId}/items/{furnitureUnitId}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteBasketItem(int basketId, Guid furnitureUnitId)
        {
            return basketAppService.DeleteBasketItemAsync(basketId, furnitureUnitId);
        }

        // PUT: synchronize baskets
        [HttpPut("{basketId}/{otherBasketId}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<int> SynchronizeBaskets(int basketId, int otherBasketId)
        {
            return basketAppService.SynchronizeBasketsAsync(basketId, otherBasketId);
        }
    }
}
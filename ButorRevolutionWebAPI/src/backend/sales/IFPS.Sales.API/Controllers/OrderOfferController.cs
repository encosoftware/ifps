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
    [Route("api/orders/{orderId}/offers")]
    [ApiController]
    public class OrderOfferController : IFPSControllerBase
    {
        private const string OPNAME = "OrdersOffer";

        private readonly IOrderAppService orderAppService;

        public OrderOfferController(
            IOrderAppService orderAppService)
        {
            this.orderAppService = orderAppService;
        }

        // PUT: Update order, create offer
        [HttpPut]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task CreateOffer(Guid orderId, [FromBody] OfferCreateDto dto)
        {
            return orderAppService.CreateOfferAsync(dto, orderId);
        }

        // Get offer details by order
        [HttpGet("details")]
        [Authorize(Policy = "GetOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<OfferDetailsDto> GetFurnitureUnitsListByOffer(Guid orderId)
        {
            return orderAppService.GetFurnitureUnitsListByOfferAsync(orderId);
        }

        // Get ordered furniture unit by orderedFurnitureUnitId
        [HttpGet("orderedfurnitureunits/{orderedFurnitureUnitId}")]
        [Authorize(Policy = "GetOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<FurnitureUnitDetailsByOfferDto> GetOrderedFurnitureUnit(Guid orderId, int orderedFurnitureUnitId)
        {
            return orderAppService.GetOrderedFurnitureUnitAsync(orderId, orderedFurnitureUnitId);
        }

        // Put: update the ordered furniture unit with quantity
        [HttpPut("orderedfurnitureunits/{orderedFurnitureUnitId}")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateOrderedFurnitureUnitQuantity(Guid orderId, int orderedFurnitureUnitId, UpdateOrderedFurnitureUnitQuantityByOfferDto dto)
        {
            return orderAppService.UpdateOrderedFurnitureUnitQuantityAsync(orderId, orderedFurnitureUnitId, dto);
        }

        // POST create new furniture unit with base furniture unit
        [HttpPost("furnitureunits/{baseFurnitureUnitId}")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> CreateWallCabinetByOffer(Guid orderId, Guid baseFurnitureUnitId, [FromBody] FurnitureUnitCreateWithQuantityByOfferDto dto)
        {
            return orderAppService.CreateFurnitureUnitByOfferAsync(orderId, baseFurnitureUnitId, dto);
        }

        // Add ordered furniture unit for order by offer
        [HttpPost("orderedfurnitureunits")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task AddFurnitureUnit(Guid orderId, [FromBody] FurnitureUnitCreateByOfferDto dto)
        {
            return orderAppService.AddOrderedFurnitureUnitAsync(orderId, dto);
        }

        // Delete: delete ordered furniture unit from list
        [HttpDelete("orderedfurnitureunits/{furnitureUnitId}")]
        [Authorize(Policy = "DeleteOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteOrderedFurnitureUnits(Guid orderId, Guid furnitureUnitId)
        {
            return orderAppService.DeleteOrderedFurnitureUnitAsync(orderId, furnitureUnitId);
        }

        // Add appliance to list by offer
        [HttpPost("appliances")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task AddAppliance(Guid orderId, [FromBody] ApplianceCreateByOfferDto dto)
        {
            return orderAppService.AddApplianceAsync(orderId, dto);
        }

        // Get appliance details for edit
        [HttpGet("appliances/{orderedApplianceMaterialId}")]
        [Authorize(Policy = "GetOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<ApplianceDetailsByOfferDto> GetAppliance(Guid orderId, int orderedApplianceMaterialId)
        {
            return orderAppService.GetApplianceAsync(orderId, orderedApplianceMaterialId);
        }

        // Update appliance by offer
        [HttpPut("appliances/{orderedApplianceMaterialId}")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateAppliance(Guid orderId, int orderedApplianceMaterialId, [FromBody] ApplianceUpdateByOfferDto dto)
        {
            return orderAppService.UpdateApplianceAsync(orderId, orderedApplianceMaterialId, dto);
        }

        // Delete appliance from list in order by offer
        [HttpDelete("appliances/{orderedApplianceMaterialId}")]
        [Authorize(Policy = "DeleteOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteApplianceFromAplliancesList(Guid orderId, int orderedApplianceMaterialId)
        {
            return orderAppService.DeleteApplianceFromAplliancesListAsync(orderId, orderedApplianceMaterialId);
        }

        // Get Offer preview
        [HttpGet("previews")]
        [Authorize(Policy = "GetOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<OfferPreviewDto> OfferPreview(Guid orderId)
        {
            return orderAppService.OfferPreview(orderId);
        }

        // Add service by offer
        [HttpPost("services")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task AddService(Guid orderId, [FromBody] ServiceCreateByOfferDto dto)
        {
            return orderAppService.AddServiceAsync(orderId, dto);
        }

        // Set VAT by offer
        [HttpPut("vat")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task SetVat(Guid orderId, bool isVat)
        {
            return orderAppService.SetVatAsync(orderId, isVat);
        }
    }
}
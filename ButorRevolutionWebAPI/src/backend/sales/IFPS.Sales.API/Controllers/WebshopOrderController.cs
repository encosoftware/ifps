using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/webshoporders")]
    [ApiController]
    public class WebshopOrderController : IFPSControllerBase
    {
        private const string OPNAME = "WebshopOrders";
        private readonly IWebshopOrdersAppService webshopOrdersAppService;

        public WebshopOrderController(IWebshopOrdersAppService webshopOrdersAppService)
        {
            this.webshopOrdersAppService = webshopOrdersAppService;
        }

        // GET: get orders by webshop
        [HttpGet("{customerId}")]
        [SwaggerOperation(Tags = new[] { OPNAME })]

        public Task<List<WebshopOrdersListDto>> GetOrdersByCustomerIdByWebshop(int customerId)
        {
            return webshopOrdersAppService.GetWebshopOrdersByCustomerIdByWebshopAsync(customerId);
        }

        // GET: get order details by webshop
        [HttpGet("{webshopOrderId}/orderedFurnitureUnits")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<WebshopOrdersDetailsDto> GetOrderedFurnitureUnitsDetailsAsync(Guid webshopOrderId)
        {
            return webshopOrdersAppService.GetOrderedFurnitureUnitsDetailsAsync(webshopOrderId);
        }
    }
}

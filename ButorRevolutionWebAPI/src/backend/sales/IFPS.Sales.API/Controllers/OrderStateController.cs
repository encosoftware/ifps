using System.Collections.Generic;
using System.Threading.Tasks;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/orderstates")]
    [ApiController]
    public class OrderStateController : IFPSControllerBase
    {
        private const string OPNAME = "OrderState";

        private readonly IOrderStateAppService orderStateAppService;
        public OrderStateController(IOrderStateAppService orderStateAppService)
        {
            this.orderStateAppService = orderStateAppService;
        }

        // GET OrderState list
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<OrderStateDto>> GetOrderStatuses()
        {
            return await orderStateAppService.GetOrderStatesAsync();
        }
    }
}
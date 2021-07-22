using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/orderstatustypes")]
    [ApiController]
    public class OrderStatusTypeController : IFPSControllerBase
    {
        private const string OPNAME = "OrderStatusTypes";

        private readonly IOrderStateTypeAppService orderStateTypeAppService;

        public OrderStatusTypeController(IOrderStateTypeAppService orderStateTypeAppService)
        {
            this.orderStateTypeAppService = orderStateTypeAppService;
        }

        // Order status list for dropdown
        [HttpGet("dropdown")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<OrderStateTypeDropdownListDto>> OrderStatusTypeDropdownList()
        {
            return orderStateTypeAppService.GetOrderStateTypeDropdownListAsync();
        }

        // GET Order status list for order scheduling
        [HttpGet("statuses/orderscheduling")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<OrderStateTypeDropdownListDto>> GetOrderSchedulingOrderStates()
        {
            return orderStateTypeAppService.GetOrderSchedulingOrderStatesAsync();
        }

        // GET Order status list for finances' orders
        [HttpGet("statuses/finances")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<OrderStateTypeDropdownListDto>> GetFinanceOrderStates()
        {
            return orderStateTypeAppService.GetFinanceOrderStatesAsync();
        }
    }
}

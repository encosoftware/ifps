using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/orderschedulings")]
    [ApiController]
    public class OrderSchedulingController : IFPSControllerBase
    {
        private const string OPNAME = "OrderSchedulings";
        private readonly IOrderSchedulingAppService orderSchedulingAppService;

        public OrderSchedulingController(IOrderSchedulingAppService orderSchedulingAppService)
        {
            this.orderSchedulingAppService = orderSchedulingAppService;
        }

        // Get all OrderScheduling
        [HttpGet]
        [Authorize(Policy = "GetOrderSchedulings")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<OrderSchedulingListDto>> OrderSchedulingList([FromQuery]OrderSchedulingFilterDto dto)
        {
            return orderSchedulingAppService.OrderSchedulingListAsync(dto);
        }

        // GET order by id for under production status
        [HttpGet("{orderId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<ProductionStatusDetailsDto> GetProductionStatusByOrderId(Guid orderId)
        {
            return orderSchedulingAppService.GetProductionStatusByOrderIdAsync(orderId);
        }
    }
}

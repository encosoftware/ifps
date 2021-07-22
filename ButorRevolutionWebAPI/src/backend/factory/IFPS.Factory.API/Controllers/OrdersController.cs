using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ENCO.DDD.Application.Dto;
using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IFPS.Factory.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : IFPSControllerBase
    {
        private const string OPNAME = "Orders";

        private readonly IOrderAppService orderAppService;

        public OrdersController(
           IOrderAppService orderAppService)
        {
            this.orderAppService = orderAppService;
        }

        // GET order name list
        [HttpGet("names")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task<List<OrderNameListDto>> GetOrderNamesAsync()
        {
            return await orderAppService.GetOrderNamesAsync();
        }

        // PUT: update order with payment
        [HttpPut("{orderId}/finances")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task AddOrderPayment(Guid orderId, [FromBody] OrderFinanceCreateDto orderFinanceCreateDto)
        {
            return orderAppService.AddOrderPaymentAsync(orderId, orderFinanceCreateDto, GetCallerId());
        }

        // GET order finance list by company
        [HttpGet("finances/{companyId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<OrderFinanceListDto>> GetOrdersByCompany(int companyId, [FromQuery]OrderFinanceFilterDto filter)
        {
            return orderAppService.GetOrdersByCompanyAsync(companyId, filter);
        }

        [HttpGet("export/orders")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public async Task ExportOrdersByCompanyAsync([FromQuery]OrderFinanceFilterDto filter)
        {
            await orderAppService.ExportOrdersResultsAsync(filter);
        }


        //GET order by id
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<OrderDetailsDto> GetById(Guid id)
        {
            return orderAppService.GetOrderDetailsAsync(id);
        }

        // TODO - kell-e egyáltalán?
        // UPDATE reserve or free up stockedmaterial
        [HttpPut("{orderId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task ReserveOrFreeUpMaterialsForOrder(Guid orderId, bool isReserved)
        {
            return orderAppService.ReserveOrFreeUpMaterialsForOrderAsync(orderId, isReserved, GetCallerId());
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> CreateOrder(OrderCreateDto dto)
        {
            return orderAppService.CreateOrderAsync(dto);
        }
    }
}
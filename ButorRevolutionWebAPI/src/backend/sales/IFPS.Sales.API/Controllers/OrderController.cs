using ENCO.DDD.Application.Dto;
using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Dto.Users;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : IFPSControllerBase
    {
        private const string OPNAME = "Orders";

        private readonly IOrderAppService orderAppService;

        public OrderController(
            IOrderAppService orderAppService)
        {
            this.orderAppService = orderAppService;
        }

        // GET order list for Admin
        [HttpGet]
        [Authorize(Policy = "GetOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<OrderListDto>> GetOrders([FromQuery]OrderFilterDto filter)
        {
            return orderAppService.GetOrdersAsync(filter, GetCallerId(), DivisionTypeEnum.Admin);
        }

        // GET order list for Sales
        [HttpGet("sales/orders")]
        [Authorize(Policy = "GetOrdersBySales")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<OrderListDto>> GetOrdersBySales([FromQuery]OrderFilterDto filter)
        {
            return orderAppService.GetOrdersAsync(filter, GetCallerId(), DivisionTypeEnum.Sales);
        }

        // GET order list for customer
        [HttpGet("customers")]
        [Authorize(Policy = "GetOrdersByCustomer")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<OrderListDto>> GetOrdersByCustomer([FromQuery]OrderFilterDto filter)
        {
            return orderAppService.GetOrdersAsync(filter, GetCallerId(), DivisionTypeEnum.Customer);
        }

        // GET order list for Partner
        [HttpGet("partner")]
        [Authorize(Policy = "GetOrdersByPartner")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<OrderListDto>> GetOrdersByPartner([FromQuery]OrderFilterDto filter)
        {
            return orderAppService.GetOrdersAsync(filter, GetCallerId(), DivisionTypeEnum.Partner);
        }        

        // TODO erre itt szükség van? ez nem a factoryban van amúgy?
        // GET order finance list by company
        [HttpGet("finances/{companyId}")]
        [Authorize(Policy = "GetOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<PagedListDto<OrderFinanceListDto>> GetOrdersByCompany(int companyId, [FromQuery]OrderFinanceFilterDto filter)
        {
            return orderAppService.GetOrdersByCompanyAsync(companyId, filter);
        }

        //GET order by id
        [HttpGet("{id}")]
        [Authorize]       
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<OrderSalesHeaderDto> GetById(Guid id)
        {
            return orderAppService.GetOrderDetailsAsync(id, GetCallerId());
        }

        //DELETE order by id
        [HttpDelete("{id}")]
        [Authorize(Policy = "DeleteOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task DeleteOrder(Guid id)
        {
            return orderAppService.DeleteOrderAsync(id);
        }

        // POST: api/Orders
        [HttpPost]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<Guid> Post([FromBody] OrderCreateDto orderDto)
        {
            return orderAppService.CreateOrderAsync(orderDto);
        }

        // PUT: api/EditOrder
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task UpdateOrder(Guid id, [FromBody] OrderEditDto updateDto)
        {
            return orderAppService.UpdateOrderState(id, updateDto);
        }


        // PUT: update order with payment
        [HttpPut("{orderId}/finances")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task AddOrderPayment(Guid orderId, OrderFinanceCreateDto orderFinanceCreateDto)
        {
            return orderAppService.AddOrderPaymentAsync(orderId, orderFinanceCreateDto);
        }

        // GET: contract
        [HttpGet("{orderId}/contract")]
        [Authorize(Policy = "GetOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<ContractDetailsDto> GetContract(Guid orderId)
        {
            return orderAppService.GetContractAsync(orderId);
        }

        // POST: create contract
        [HttpPost("{orderId}/contract")]
        [Authorize(Policy = "UpdateOrders")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task CreateContract(Guid orderId,[FromBody] ContractCreateDto dto)
        {
            return orderAppService.CreateContractAsync(orderId, dto);
        }

        //GET: get customer by OrderId //id, Name
        [HttpGet("{orderId}/customer")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<UserDropdownAvatarDto> GetCustomerByOrderId(Guid orderId)
        {
            return orderAppService.GetCustomerByOrderAsync(orderId);
        }

        // PUT: set shipping state 
        [HttpPut("{orderId}/states/shipping")]
        [Authorize(Policy = "UpdateOrdersByPartner")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task SetShippinState(Guid orderId)
        {
            return orderAppService.SetShippingStateAsync(orderId);
        }

        // PUT: set installation state 
        [HttpPut("{orderId}/states/installation")]
        [Authorize(Policy = "UpdateOrdersByPartner")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task SetInstallationState(Guid orderId)
        {
            return orderAppService.SetInstallationStateAsync(orderId);
        }

        // PUT: set on-site survey state
        [HttpPut("{orderId}/states/onsitesurvey")]
        [Authorize(Policy = "UpdateOrdersByPartner")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task SetOnSiteSurveyState(Guid orderId)
        {
            return orderAppService.SetWaitingForContractStateAsync(orderId, GetCallerId());
        }
    }
}
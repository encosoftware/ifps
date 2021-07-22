using IFPS.Sales.API.Common;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.API.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : IFPSControllerBase
    {
        private const string OPNAME = "Tickets";

        private readonly ITicketAppService ticketAppService;

        public TicketsController(ITicketAppService ticketAppService)
        {
            this.ticketAppService = ticketAppService;
        }

        // GET tickets
        [HttpGet]
        [Authorize(Policy = "GetTickets")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<TicketListDto>> GetTicketList()
        {
            return ticketAppService.GetTicketList();
        }

        // GET own tickets
        [HttpGet("own")]
        [Authorize(Policy = "GetTickets")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<TicketListDto>> GetOwnTicketList()
        {
            return ticketAppService.GetOwnTicketList(GetCallerId());
        }

        // GET tickets by order
        [HttpGet("orders/{orderId}")]
        [Authorize(Policy = "GetTickets")]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<TicketByOrderListDto>> GetTicketsByOrder(Guid orderId)
        {
            return ticketAppService.GetTicketsByOrderAsync(orderId);
        }

        //GET tickets by order for customer
        [HttpGet("own/order/{orderId}")]
        //TODO: policy?
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<TicketByOrderListDto>> GetCustomerTicketsByOrder(Guid orderId)
        {
            return ticketAppService.GetCustomerTicketsByOrderAsync(orderId, GetCallerId());
        }
    }
}
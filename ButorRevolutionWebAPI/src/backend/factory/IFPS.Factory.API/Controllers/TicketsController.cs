using IFPS.Factory.API.Common;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.API.Controllers
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

        // GET tickets by order
        [HttpGet("orders/{orderId}")]
        [Authorize]
        [SwaggerOperation(Tags = new[] { OPNAME })]
        public Task<List<TicketByOrderListDto>> GetTicketsByOrder(Guid orderId)
        {
            return ticketAppService.GetTicketsByOrderAsync(orderId);
        }
    }
}
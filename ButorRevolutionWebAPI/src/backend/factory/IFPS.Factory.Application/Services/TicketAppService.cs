using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class TicketAppService : ApplicationService, ITicketAppService
    {
        private readonly IOrderRepository orderRepository;

        public TicketAppService(IApplicationServiceDependencyAggregate aggregate,
            IOrderRepository orderRepository)
            : base(aggregate)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<List<TicketByOrderListDto>> GetTicketsByOrderAsync(Guid orderId)
        {
            var order = await orderRepository.GetOrderWithTicketsAsync(orderId);
            var archivedTickets = order.Tickets.Select(ent => new TicketByOrderListDto(ent)).ToList();
            if(order.CurrentTicket != null) archivedTickets.Add(new TicketByOrderListDto(order.CurrentTicket));
            return archivedTickets.ToList();
        }
    }
}

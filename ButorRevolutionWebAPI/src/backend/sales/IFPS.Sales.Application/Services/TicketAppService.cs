using ENCO.DDD.Service;
using IFPS.Sales.Application.Dto;
using IFPS.Sales.Application.Interfaces;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Application.Services
{
    public class TicketAppService : ApplicationService, ITicketAppService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserTeamRepository userTeamRepository;

        public TicketAppService(IApplicationServiceDependencyAggregate aggregate,
            IOrderRepository orderRepository,
            IUserTeamRepository userTeamRepository)
            : base(aggregate)
        {
            this.orderRepository = orderRepository;
            this.userTeamRepository = userTeamRepository;
        }

        public async Task<List<TicketListDto>> GetOwnTicketList(int userId)
        {
            var userIds = await userTeamRepository.GetTechnicalUserIdsByUserIdAsync(userId);
            userIds.Add(userId);
            return await orderRepository.GetAllListAsync(ent => userIds.Contains(ent.CurrentTicket.AssignedToId.Value), TicketListDto.Projection);            
        }

        public async Task<List<TicketListDto>> GetTicketList()
        {
            return await orderRepository.GetAllListAsync(ent => 
            ent.CurrentTicket.OrderState.State != Domain.Enums.OrderStateEnum.Completed, TicketListDto.Projection);
        }

        public async Task<List<TicketByOrderListDto>> GetTicketsByOrderAsync(Guid orderId)
        {
            var order = await orderRepository.GetOrderWithTicketsAsync(orderId);
            if (order.Tickets.Any())
                return order.Tickets.Select(ent => new TicketByOrderListDto(ent)).OrderBy(ent => ent.ClosedOn).ToList();
            else return new List<TicketByOrderListDto> { new TicketByOrderListDto(order.CurrentTicket) };
        }

        public async Task<List<TicketByOrderListDto>> GetCustomerTicketsByOrderAsync(Guid orderId, int customerId)
        {
            var order = await orderRepository.GetCustomerOrderWithTicketsAsync(orderId, customerId);
            if (order.Tickets.Any())
                return order.Tickets.Select(ent => new TicketByOrderListDto(ent)).OrderBy(ent => ent.ClosedOn).ToList();
            else return new List<TicketByOrderListDto> { new TicketByOrderListDto(order.CurrentTicket) };
        }
    }
}

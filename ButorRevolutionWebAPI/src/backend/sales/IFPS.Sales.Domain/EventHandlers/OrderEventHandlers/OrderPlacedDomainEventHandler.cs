using IFPS.Sales.Domain.Events.OrderEvents;
using IFPS.Sales.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.EventHandlers.OrderEventHandlers
{
    public class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedDomainEvent>
    {
        private readonly IOrderRepository orderRepository;
        public OrderPlacedDomainEventHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public async Task Handle(OrderPlacedDomainEvent notification, CancellationToken cancellationToken)
        {
            notification.Order.SetWorkingNumber(Clock.Now.Year, await orderRepository.GetNextWorkingNumber(Clock.Now.Year));
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Events;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;

namespace IFPS.Sales.Domain.EventHandlers
{
    public class DeliveredDomainEventHandler : INotificationHandler<DeliveredDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public DeliveredDomainEventHandler(
            IOrderStateRepository orderStateRepository,
            IOptions<OrderStateDeadlineConfiguration> orderStateConfiguration)
        {
            this.orderStateRepository = orderStateRepository;
            this.orderStateConfiguration = orderStateConfiguration.Value;
        }

        public async Task Handle(DeliveredDomainEvent notification, CancellationToken cancellationToken)
        {
            var deliveredState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.Delivered);
            var delivered = new Ticket(deliveredState.Id, notification.PartnerId, orderStateConfiguration.Delivered);

            notification.Order.AddTicket(delivered);
        }
    }
}

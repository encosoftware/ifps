using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Events;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.EventHandlers
{
    public class WaitingForShippingAppointmentReservationDomainEventHandler : INotificationHandler<WaitingForShippingAppointmentReservationDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public WaitingForShippingAppointmentReservationDomainEventHandler(
            IOrderStateRepository orderStateRepository,
            IOptions<OrderStateDeadlineConfiguration> orderStateConfiguration)
        {
            this.orderStateRepository = orderStateRepository;
            this.orderStateConfiguration = orderStateConfiguration.Value;
        }

        public async Task Handle(WaitingForShippingAppointmentReservationDomainEvent notification, CancellationToken cancellationToken)
        {
            var waitingForShippingAppointmentResrevationState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForShippingAppointmentReservation);
            var waitingForShippingAppointmentResrevation = new Ticket(waitingForShippingAppointmentResrevationState.Id, notification.PartnerId, orderStateConfiguration.WaitingForShippingAppointmentReservation);

            notification.Order.AddTicket(waitingForShippingAppointmentResrevation);
        }
    }
}

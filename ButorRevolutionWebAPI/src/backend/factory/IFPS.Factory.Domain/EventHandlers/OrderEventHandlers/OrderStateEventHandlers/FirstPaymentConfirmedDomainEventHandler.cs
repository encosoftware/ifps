using System.Threading;
using System.Threading.Tasks;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Events;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using MediatR;

namespace IFPS.Factory.Domain.EventHandlers
{
    public class FirstPaymentConfirmedDomainEventHandler : INotificationHandler<FirstPaymentConfirmedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;

        public FirstPaymentConfirmedDomainEventHandler(IOrderStateRepository orderStateRepository)
        {
            this.orderStateRepository = orderStateRepository;
        }

        public async Task Handle(FirstPaymentConfirmedDomainEvent notification, CancellationToken cancellationToken)
        {
            var firstPaymentConfirmedState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.FirstPaymentConfirmed);
            var underMaterialReservationState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.UnderMaterialReservation);

            var firstPaymentConfirmed = new Ticket(firstPaymentConfirmedState.Id, notification.UserId, notification.Order.Deadline);
            var underMaterialReservation = new Ticket(underMaterialReservationState.Id, notification.UserId, notification.Order.Deadline);

            notification.Order.AddTicket(firstPaymentConfirmed);
            notification.Order.AddTicket(underMaterialReservation);
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Events;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using MediatR;

namespace IFPS.Factory.Domain.EventHandlers
{
    public class UnderMaterialReservationDomainEventHandler : INotificationHandler<UnderMaterialReservationDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;

        public UnderMaterialReservationDomainEventHandler(IOrderStateRepository orderStateRepository)
        {
            this.orderStateRepository = orderStateRepository;
        }

        public async Task Handle(UnderMaterialReservationDomainEvent notification, CancellationToken cancellationToken)
        {
            var underMaterialReservationState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.UnderMaterialReservation);

            var underMaterialReservation = new Ticket(underMaterialReservationState.Id, notification.UserId, notification.Order.Deadline);

            notification.Order.AddTicket(underMaterialReservation);
        }
    }
}

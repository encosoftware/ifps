using System.Threading;
using System.Threading.Tasks;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Events;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using MediatR;

namespace IFPS.Factory.Domain.EventHandlers
{
    public class WaitingForSchedulingDomainEventHandler : INotificationHandler<WaitingForSchedulingDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;

        public WaitingForSchedulingDomainEventHandler(IOrderStateRepository orderStateRepository)
        {
            this.orderStateRepository = orderStateRepository;
        }

        public async Task Handle(WaitingForSchedulingDomainEvent notification, CancellationToken cancellationToken)
        {
            var waitingForSchedulingState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForScheduling);

            var waitingForScheduling = new Ticket(waitingForSchedulingState.Id, notification.UserId, notification.Order.Deadline);

            notification.Order.AddTicket(waitingForScheduling);
        }
    }
}

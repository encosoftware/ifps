using System.Threading;
using System.Threading.Tasks;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Events;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using MediatR;

namespace IFPS.Factory.Domain.EventHandlers
{
    public class ScheduledDomainEventHandler : INotificationHandler<ScheduledDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;

        public ScheduledDomainEventHandler(IOrderStateRepository orderStateRepository)
        {
            this.orderStateRepository = orderStateRepository;
        }

        public async Task Handle(ScheduledDomainEvent notification, CancellationToken cancellationToken)
        {
            var scheduledState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.Scheduled);

            var scheduled = new Ticket(scheduledState.Id, notification.UserId, notification.Order.Deadline);

            notification.Order.AddTicket(scheduled);
        }
    }
}

using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Events;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.EventHandlers
{
    public class UnderProductionDomainEventHandler : INotificationHandler<UnderProductionDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;

        public UnderProductionDomainEventHandler(IOrderStateRepository orderStateRepository)
        {
            this.orderStateRepository = orderStateRepository;
        }

        public async Task Handle(UnderProductionDomainEvent notification, CancellationToken cancellationToken)
        {
            var underProductionState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.UnderProduction);

            var underProduction = new Ticket(underProductionState.Id, notification.UserId, notification.Order.Deadline);

            notification.Order.AddTicket(underProduction);
        }
    }
}

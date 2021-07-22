using System.Threading;
using System.Threading.Tasks;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Events;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using MediatR;

namespace IFPS.Factory.Domain.EventHandlers
{
    public class WaitingForSecondPaymentDomainEventHandler : INotificationHandler<WaitingForSecondPaymentDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;

        public WaitingForSecondPaymentDomainEventHandler(IOrderStateRepository orderStateRepository)
        {
            this.orderStateRepository = orderStateRepository;
        }

        public async Task Handle(WaitingForSecondPaymentDomainEvent notification, CancellationToken cancellationToken)
        {
            var productionCompleteState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.ProductionComplete);
            var waitingForSecondPaymentState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForSecondPayment);

            var productionComplete = new Ticket(productionCompleteState.Id, notification.UserId, notification.Order.Deadline);
            var waitingForSecondPayment = new Ticket(waitingForSecondPaymentState.Id, notification.UserId, notification.Order.Deadline);

            notification.Order.AddTicket(productionComplete);
            notification.Order.AddTicket(waitingForSecondPayment);
        }
    }
}

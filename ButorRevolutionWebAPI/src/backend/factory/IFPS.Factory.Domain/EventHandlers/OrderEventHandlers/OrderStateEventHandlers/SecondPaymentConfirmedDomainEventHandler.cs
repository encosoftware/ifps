using System.Threading;
using System.Threading.Tasks;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Events;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using MediatR;

namespace IFPS.Factory.Domain.EventHandlers
{
    public class SecondPaymentConfirmedDomainEventHandler : INotificationHandler<SecondPaymentConfirmedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;

        public SecondPaymentConfirmedDomainEventHandler(IOrderStateRepository orderStateRepository)
        {
            this.orderStateRepository = orderStateRepository;
        }

        public async Task Handle(SecondPaymentConfirmedDomainEvent notification, CancellationToken cancellationToken)
        {
            var secondPaymentConfirmedState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.SecondPaymentConfirmed);

            var secondPaymentConfirmed = new Ticket(secondPaymentConfirmedState.Id, notification.UserId, notification.Order.Deadline);

            notification.Order.AddTicket(secondPaymentConfirmed);
        }
    }
}

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
    public class OnSiteSurveyDoneDomainEventHandler : INotificationHandler<OnSiteSurveyDoneDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public OnSiteSurveyDoneDomainEventHandler(
            IOrderStateRepository orderStateRepository,
            IOptions<OrderStateDeadlineConfiguration> orderStateConfiguration)
        {
            this.orderStateRepository = orderStateRepository;
            this.orderStateConfiguration = orderStateConfiguration.Value;
        }

        public async Task Handle(OnSiteSurveyDoneDomainEvent notification, CancellationToken cancellationToken)
        {
            var waitinForContractState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForContract);
            var onSiteSurveyDone = new Ticket(waitinForContractState.Id, notification.PartnerId, orderStateConfiguration.OnSiteSurveyDone);

            notification.Order.AddTicket(onSiteSurveyDone);
        }
    }
}

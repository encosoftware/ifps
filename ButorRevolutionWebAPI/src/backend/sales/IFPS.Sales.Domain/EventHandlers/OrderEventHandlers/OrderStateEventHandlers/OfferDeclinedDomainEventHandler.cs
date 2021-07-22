using System.Threading;
using System.Threading.Tasks;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Events;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace IFPS.Sales.Domain.EventHandlers
{
    public class OfferDeclinedDomainEventHandler : INotificationHandler<OfferDeclinedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly ICustomerEmailService customerEmailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public OfferDeclinedDomainEventHandler(
            IOrderStateRepository orderStateRepository,
            ICustomerEmailService customerEmailService,
            IEmailDataRepository emailDataRepository,
            IOptions<OrderStateDeadlineConfiguration> orderStateConfiguration)
        {
            this.orderStateRepository = orderStateRepository;
            this.customerEmailService = customerEmailService;
            this.emailDataRepository = emailDataRepository;
            this.orderStateConfiguration = orderStateConfiguration.Value;
        }
        public async Task Handle(OfferDeclinedDomainEvent notification, CancellationToken cancellationToken)
        {
            var declinedOfferState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.OfferDeclined);
            var waitingForOfferState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForOffer);

            var declinedOffer = new Ticket(declinedOfferState.Id, notification.Order.Customer.UserId, orderStateConfiguration.OfferDeclined);
            var waitingForOffer = new Ticket(waitingForOfferState.Id, notification.Order.SalesPerson.UserId, orderStateConfiguration.WaitingForOffer);

            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.WaitingForOffer);

            notification.Order.AddTicket(declinedOffer);
            notification.Order.AddTicket(waitingForOffer);

            await customerEmailService.SendEmailToCustomerAsync(
            user: notification.Order.Customer.User,
            orderState: waitingForOfferState,
            emailDataId: emailData.Id,
            workingNumber: notification.Order.WorkingNumber);
        }
    }
}

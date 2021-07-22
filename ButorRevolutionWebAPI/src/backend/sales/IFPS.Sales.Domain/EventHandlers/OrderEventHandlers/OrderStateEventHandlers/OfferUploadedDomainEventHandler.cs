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
    public class OfferUploadedDomainEventHandler : INotificationHandler<OfferUploadedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly ICustomerEmailService customerEmailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public OfferUploadedDomainEventHandler(
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

        public async Task Handle(OfferUploadedDomainEvent notification, CancellationToken cancellationToken)
        {
            var waitingForOfferFeedbackState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForOfferFeedback);
            var offerFeedback = new Ticket(waitingForOfferFeedbackState.Id, notification.Order.SalesPerson.UserId, orderStateConfiguration.WaitingForOfferFeedback);
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.OfferUploaded);

            notification.Order.AddTicket(offerFeedback);

            await customerEmailService.SendEmailToCustomerAsync(
                user: notification.Order.Customer.User,
                orderState: waitingForOfferFeedbackState,
                emailDataId: emailData.Id,
                workingNumber: notification.Order.WorkingNumber);
        }
    }
}

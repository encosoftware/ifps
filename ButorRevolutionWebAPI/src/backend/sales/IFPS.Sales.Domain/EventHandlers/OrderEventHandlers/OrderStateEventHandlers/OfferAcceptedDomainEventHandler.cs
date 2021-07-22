using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Events;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.EventHandlers
{
    public class OfferAcceptedDomainEventHandler : INotificationHandler<OfferAcceptedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly ICustomerEmailService customerEmailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public OfferAcceptedDomainEventHandler(
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

        public async Task Handle(OfferAcceptedDomainEvent notification, CancellationToken cancellationToken)
        {
            var waitingForOnSiteSurveyAppointmentreservationState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForOnSiteSurveyAppointmentReservation);
            var offerAccepted = new Ticket(waitingForOnSiteSurveyAppointmentreservationState.Id, notification.Order.SalesPerson.UserId, orderStateConfiguration.WaitingForOnSiteSurveyAppointmentReservation);
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.WaitingForContract);

            notification.Order.AddTicket(offerAccepted);

            await customerEmailService.SendEmailToCustomerAsync(
             user: notification.Order.Customer.User,
             orderState: waitingForOnSiteSurveyAppointmentreservationState,
             emailDataId: emailData.Id,
             workingNumber: notification.Order.WorkingNumber);
        }
    }
}

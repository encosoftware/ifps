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
    public class InstallationAppointmentCreatedDomainEventHandler : INotificationHandler<InstallationAppointmentCreatedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly ICustomerEmailService customerEmailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public InstallationAppointmentCreatedDomainEventHandler(
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

        public async Task Handle(InstallationAppointmentCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var waitingForInstallationState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForInstallation);
            var waitingForInstallation = new Ticket(waitingForInstallationState.Id, notification.PartnerId, orderStateConfiguration.WaitingForInstallation);
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.DocumentUploadSuccess);

            notification.Order.AddTicket(waitingForInstallation);

            await customerEmailService.SendEmailToCustomerAsync(
            user: notification.Order.Customer.User,
            orderState: waitingForInstallationState,
            emailDataId: emailData.Id, // TODO átírni
            workingNumber: notification.Order.WorkingNumber);
        }
    }
}

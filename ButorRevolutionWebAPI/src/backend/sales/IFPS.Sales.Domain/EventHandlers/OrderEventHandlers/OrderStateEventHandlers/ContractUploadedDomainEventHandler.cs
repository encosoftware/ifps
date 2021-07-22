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
    public class ContractUploadedDomainEventHandler : INotificationHandler<ContractUploadedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly ICustomerEmailService customerEmailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public ContractUploadedDomainEventHandler(
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

        public async Task Handle(ContractUploadedDomainEvent notification, CancellationToken cancellationToken)
        {
            var waitingForContractFeedback = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForContractFeedback);
            var waitingForContract = new Ticket(waitingForContractFeedback.Id, notification.Order.SalesPerson.UserId, orderStateConfiguration.WaitingForContractFeedback);
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.ContractUploaded);

            notification.Order.AddTicket(waitingForContract);

             await customerEmailService.SendEmailToCustomerAsync(
             user: notification.Order.Customer.User,
             orderState: waitingForContractFeedback,
             emailDataId: emailData.Id,
             workingNumber: notification.Order.WorkingNumber);
        }
    }
}

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
    public class ContractDeclinedDomainEventHandler : INotificationHandler<ContractDeclinedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly ICustomerEmailService customerEmailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public ContractDeclinedDomainEventHandler(
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

        public async Task Handle(ContractDeclinedDomainEvent notification, CancellationToken cancellationToken)
        {
            var declinedContractState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.ContractDeclined);
            var waitingForContractState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForContract);

            var declinedContract = new Ticket(declinedContractState.Id, notification.Order.Customer.UserId, orderStateConfiguration.ContractDeclined);
            var waitingForContract = new Ticket(waitingForContractState.Id, notification.Order.SalesPerson.UserId, orderStateConfiguration.WaitingForContract);

            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.WaitingForContract);

            notification.Order.AddTicket(declinedContract);
            notification.Order.AddTicket(waitingForContract);

            await customerEmailService.SendEmailToCustomerAsync(
             user: notification.Order.Customer.User,
             orderState: waitingForContractState,
             emailDataId: emailData.Id,
             workingNumber: notification.Order.WorkingNumber);
        }
    }
}

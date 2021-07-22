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
    public class ContractAcceptedDomainEventHandler : INotificationHandler<ContractAcceptedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly ICustomerEmailService customerEmailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public ContractAcceptedDomainEventHandler(
            IOrderStateRepository orderStateRepository,
            ICustomerEmailService customerEmailSerivce,
            IEmailDataRepository emailDataRepository,
            IOptions<OrderStateDeadlineConfiguration> orderStateConfiguration)
        {
            this.orderStateRepository = orderStateRepository;
            this.customerEmailService = customerEmailSerivce;
            this.emailDataRepository = emailDataRepository;
            this.orderStateConfiguration = orderStateConfiguration.Value;
        }

        public async Task Handle(ContractAcceptedDomainEvent notification, CancellationToken cancellationToken)
        {
            var underProductionState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.UnderProduction);
            var contractSignedState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.ContractSigned);

            var underProduction = new Ticket(underProductionState.Id, notification.Order.SalesPerson.UserId, orderStateConfiguration.UnderProduction);
            var contractSigned = new Ticket(contractSignedState.Id, notification.Order.Customer.UserId, orderStateConfiguration.ContractSigned);

            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.ContractSignedState);

            notification.Order.AddTicket(contractSigned);
            notification.Order.AddTicket(underProduction);

            await customerEmailService.SendEmailToCustomerAsync(
             user: notification.Order.Customer.User,
             orderState: contractSignedState,
             emailDataId: emailData.Id,
             workingNumber: notification.Order.WorkingNumber);
        }
    }
}

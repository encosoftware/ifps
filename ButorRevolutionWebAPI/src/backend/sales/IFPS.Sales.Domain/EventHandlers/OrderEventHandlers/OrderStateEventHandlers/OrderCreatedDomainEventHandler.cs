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
    public class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
    {
        private readonly IOrderStateRepository orderStateRepository;
        private readonly ICustomerEmailService customerEmailService;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly OrderStateDeadlineConfiguration orderStateConfiguration;

        public OrderCreatedDomainEventHandler(
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

        public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var createdOrderState = await orderStateRepository.SingleAsync(ent => ent.State == OrderStateEnum.WaitingForOffer);
            var createdOrder = new Ticket(createdOrderState.Id, notification.Order.SalesPerson.UserId, orderStateConfiguration.OrderCreated);
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == EmailTypeEnum.WaitingForOffer);

            notification.Order.AddTicket(createdOrder);

            await customerEmailService.SendEmailToCustomerAsync(
            user: notification.Order.Customer.User,
            orderState: createdOrderState,
            emailDataId: emailData.Id,
            workingNumber: notification.Order.WorkingNumber);
        }
    }
}

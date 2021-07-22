using IFPS.Sales.Domain.Events.OrderEvents;
using IFPS.Sales.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.EventHandlers.OrderEventHandlers
{
    public class WebshopOrderPlacedDomainEventHandler : INotificationHandler<WebshopOrderPlacedDomainEvent>
    {
        private readonly IWebshopOrderRepository webshopOrderRepository;
        public WebshopOrderPlacedDomainEventHandler(IWebshopOrderRepository webshopOrderRepository)
        {
            this.webshopOrderRepository = webshopOrderRepository;
        }
        public async Task Handle(WebshopOrderPlacedDomainEvent notification, CancellationToken cancellationToken)
        {
            notification.WebshopOrder.SetWorkingNumber(Clock.Now.Year, await webshopOrderRepository.GetNextWorkingNumber(Clock.Now.Year));
        }        
    }
}
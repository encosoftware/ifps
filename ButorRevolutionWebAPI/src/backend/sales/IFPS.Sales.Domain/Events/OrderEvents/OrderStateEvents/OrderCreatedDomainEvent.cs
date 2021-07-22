using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Events
{
    public class OrderCreatedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }

        public OrderCreatedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}

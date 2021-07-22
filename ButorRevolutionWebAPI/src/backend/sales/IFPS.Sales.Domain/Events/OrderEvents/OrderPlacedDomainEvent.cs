using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Events.OrderEvents
{
    public class OrderPlacedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }

        public OrderPlacedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}

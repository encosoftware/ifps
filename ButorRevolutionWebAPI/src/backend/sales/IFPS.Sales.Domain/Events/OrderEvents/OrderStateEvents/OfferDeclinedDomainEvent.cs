using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Events
{
    public class OfferDeclinedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }

        public OfferDeclinedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}

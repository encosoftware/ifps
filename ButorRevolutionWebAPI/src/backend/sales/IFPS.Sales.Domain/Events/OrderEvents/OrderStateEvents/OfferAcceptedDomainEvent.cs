using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Events
{
    public class OfferAcceptedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }

        public OfferAcceptedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}

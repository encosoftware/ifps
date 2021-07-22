using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Events
{
    public class OfferUploadedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }

        public OfferUploadedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}

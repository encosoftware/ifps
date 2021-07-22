using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Events
{
    public class ShippingAppointmentCreatedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }
        public int PartnerId { get; private set; }

        public ShippingAppointmentCreatedDomainEvent(Order order, int partnerId)
        {
            this.Order = order;
            this.PartnerId = partnerId;
        }
    }
}

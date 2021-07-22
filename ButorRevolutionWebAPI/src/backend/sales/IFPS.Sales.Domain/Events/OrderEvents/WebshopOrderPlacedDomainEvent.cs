using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Events.OrderEvents
{
    public class WebshopOrderPlacedDomainEvent : IIFPSDomainEvent
    {
        public WebshopOrder WebshopOrder { get; private set; }

        public WebshopOrderPlacedDomainEvent(WebshopOrder webshopOrder)
        {
            this.WebshopOrder = webshopOrder;
        }
    }
}

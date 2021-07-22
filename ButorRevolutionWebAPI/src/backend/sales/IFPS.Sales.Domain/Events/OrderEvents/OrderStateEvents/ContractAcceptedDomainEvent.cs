using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Events
{
    public class ContractAcceptedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }

        public ContractAcceptedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}

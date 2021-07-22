using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Events
{
    public class ContractDeclinedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }

        public ContractDeclinedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}

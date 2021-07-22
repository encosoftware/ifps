using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Events
{
    public class ContractUploadedDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }

        public ContractUploadedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}

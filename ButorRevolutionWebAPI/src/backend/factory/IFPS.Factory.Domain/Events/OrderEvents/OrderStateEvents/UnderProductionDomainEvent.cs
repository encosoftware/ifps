using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Events
{
    public class UnderProductionDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; set; }
        public int UserId { get; set; }

        public UnderProductionDomainEvent(Order order, int userId)
        {
            Order = order;
            UserId = userId;
        }
    }
}

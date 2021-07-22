using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Events
{
    public class ScheduledDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }
        public int UserId { get; private set; }

        public ScheduledDomainEvent(Order order, int userId)
        {
            this.Order = order;
            this.UserId = userId;
        }
    }
}

using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Events
{
    public class WaitingForSecondPaymentDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }
        public int UserId { get; private set; }

        public WaitingForSecondPaymentDomainEvent(Order order, int userId)
        {
            this.Order = order;
            this.UserId = userId;
        }
    }
}

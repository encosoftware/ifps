using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Events
{
    public class UnderMaterialReservationDomainEvent : IIFPSDomainEvent
    {
        public Order Order { get; private set; }
        public int UserId { get; private set; }

        public UnderMaterialReservationDomainEvent(Order order, int userId)
        {
            Order = order;
            UserId = userId;
        }
    }
}

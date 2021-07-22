using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class Ticket : FullAuditedEntity
    {
        /// <summary>
        /// State of the ticket
        /// </summary>
        public int OrderStateId { get; private set; }
        public OrderState OrderState { get; private set; }

        /// <summary>
        /// User, responsible for the ticket
        /// </summary>
        public virtual User AssignedTo { get; set; }
        public int? AssignedToId { get; set; }

        public DateTime? ValidTo { get; set; }

        public Order Order { get; set; }
        public Guid? OrderId { get; set; }

        /// <summary>
        /// Deadline of the ticket
        /// </summary>
        public DateTime? Deadline { get; set; }

        private Ticket()
        {

        }

        public Ticket(int orderStateId, int? assignedTo, DateTime? deadline, Guid? orderId = null, DateTime? validTo = null) : this()
        {
            AssignedToId = assignedTo;
            OrderStateId = orderStateId;
            Deadline = deadline;
            OrderId = orderId;
            ValidTo = validTo;
        }

        public Ticket(OrderState orderState, User assignedTo, DateTime? deadline, Order order = null) : this()
        {
            OrderState = orderState;
            AssignedTo = assignedTo;
            Deadline = deadline;
            Order = order;
        }

        public void Close(Order order)
        {
            ValidTo = Clock.Now;
            Order = order;
        }

        public void Close(Guid orderId)
        {
            ValidTo = Clock.Now;
            OrderId = orderId;
        }

        public void Reschedule(DateTime deadline)
        {
            Deadline = deadline;
        }
    }
}

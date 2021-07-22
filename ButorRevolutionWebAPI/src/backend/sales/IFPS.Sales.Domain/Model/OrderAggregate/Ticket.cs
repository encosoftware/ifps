using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Sales.Domain.Model
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
        public virtual User AssignedTo { get; private set; }
        public int? AssignedToId { get; private set; }

        public DateTime? ValidTo { get; private set; }

        public Order Order { get; private set; }
        public Guid? OrderId { get; private set; }

        /// <summary>
        /// Deadline of the ticket
        /// </summary>
        public DateTime? Deadline { get; private set; }

        private Ticket()
        {

        }

        public Ticket(int orderStateId, int? assignedTo, int deadlineOffset, Guid? orderId = null, DateTime? validTo = null) : this()
        {
            AssignedToId = assignedTo;            
            OrderStateId = orderStateId;
            Deadline = Clock.Now.AddDays(deadlineOffset);
            OrderId = orderId;
            ValidTo = validTo;
        }
        public Ticket(OrderState orderState, User assignedTo, int deadlineOffset, Order order = null) : this()
        {
            OrderState = orderState;
            AssignedTo = assignedTo;
            Deadline = Clock.Now.AddDays(deadlineOffset);
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

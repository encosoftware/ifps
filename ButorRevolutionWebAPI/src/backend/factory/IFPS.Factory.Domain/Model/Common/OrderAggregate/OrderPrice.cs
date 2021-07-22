using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Factory.Domain.Model.Interfaces;
using System;

namespace IFPS.Factory.Domain.Model
{
    public class OrderPrice : FullAuditedEntity, IEntityPrice<Order, Guid>
    {
        /// <summary>
        /// Furniture unit
        /// </summary>
        public Order Core { get; set; }
        public Guid? CoreId { get; set; }

        /// <summary>
        /// Price of the furniture unit
        /// </summary>
        public Price Price { get; set; }

        /// <summary>
        /// Deadline of the payment
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Date of the payment
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Date, from which the price is valid
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Date, up to which the price is valid
        /// </summary>
        public DateTime? ValidTo { get; set; }

        public OrderPrice() { }

        public OrderPrice(Price price, DateTime deadline)
        {
            Price = price;
            Deadline = deadline;
            ValidFrom = DateTime.Now;
        }

        public void SetPaymentDate(DateTime date)
        {
            PaymentDate = date;
        }
    }
}

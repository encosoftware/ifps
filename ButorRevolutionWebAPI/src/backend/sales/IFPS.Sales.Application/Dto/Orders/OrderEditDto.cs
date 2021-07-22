using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class OrderEditDto
    {
        public int CurrentStatusId { get; set; }
        public DateTime StatusDeadline { get; set; }
        public int? AssignedToUserId { get; set; }
        public int CustomerUserId { get; set; }
        public AddressCreateDto ShippingAddress { get; set; }
        public int SalesPersonUserId { get; set; }
        public DateTime Deadline { get; set; }

        public Order UpdateModelObject(Order order, int customerId, int salesPersonId, int stateId, int userId, int deadlineOffset)
        {
            var address = ShippingAddress.CreateModelObject();

            if (!order.ShippingAddress.Equals(address))
            {
                order.ShippingAddress = address;
            }

            if (order.CurrentTicket.OrderStateId != stateId)
            {
                order.AddTicket(new Ticket(stateId, userId, deadlineOffset, order.Id, Deadline.AddDays(-5)));
            }

            if (!order.Deadline.Equals(Deadline))
            {
                order.Deadline = Deadline;
            }

            if (order.CustomerId != customerId)
            {
                order.CustomerId = customerId;
            }

            if (order.SalesPersonId != salesPersonId)
            {
                order.SalesPersonId = salesPersonId;
            }

            return order;
        }
    }
}

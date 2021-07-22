using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class OrderCreateDto
    {
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public DateTime Deadline { get; set; }
        public List<Guid> FurnitureUnitIds { get; set; }
        public List<Guid> ApplianceIds { get; set; }

        public Order CreateModelObject()
        {
            return new Order(OrderName) { WorkingNumber = WorkingNumber, Deadline = Deadline, CurrentTicketId = 1 };
        }
    }

}

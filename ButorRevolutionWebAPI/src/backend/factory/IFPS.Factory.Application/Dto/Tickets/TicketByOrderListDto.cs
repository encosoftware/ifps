using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class TicketByOrderListDto
    {
        public string State { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? ClosedOn { get; set; }
        public DateTime? Deadline { get; set; }

        public TicketByOrderListDto(Ticket ticket)
        {
            State = ticket.OrderState.CurrentTranslation.Name;
            AssignedTo = ticket.AssignedTo.CurrentVersion.Name;
            ClosedOn = ticket.ValidTo;
            Deadline = ticket.Deadline;
        }
    }
}

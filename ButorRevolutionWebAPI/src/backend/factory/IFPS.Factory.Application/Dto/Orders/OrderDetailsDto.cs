using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class OrderDetailsDto
    {
        public string OrderId { get; set; }
        public OrderStateDto CurrentStatus { get; set; }
        public DateTime? StatusDeadline { get; set; }
        public UserDto Responsible { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Deadline { get; set; }
        public OrderFinanceDetailsDto Finances { get; set; }
        public string WorkingNumber { get; set; }

        public static Func<Order, OrderDetailsDto> FromEntity = entity => new OrderDetailsDto
        {
            OrderId = entity.OrderName,
            CurrentStatus = OrderStateDto.FromEntity(entity.CurrentTicket.OrderState),
            StatusDeadline = entity.CurrentTicket.Deadline,
            Responsible = entity.CurrentTicket.AssignedTo != null ? new UserDto(entity.CurrentTicket.AssignedTo) : null,
            CreatedOn = entity.CreationTime,
            Deadline = entity.Deadline,
            Finances = OrderFinanceDetailsDto.FromEntity(entity),
            WorkingNumber = entity.WorkingNumber,
        };       
    }
}

using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class OrderSchedulingListDto
    {
        public string OrderName { get; set; }
        public Guid OrderId { get; set; }
        public string WorkingNumber { get; set; }
        public OrderStateListDto CurrentStatus { get; set; }
        public int EstimatedProcessTime { get; set; }
        public double Completion { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsEnough { get; set; }

        public OrderSchedulingListDto() { }

        public static Func<Order, OrderSchedulingListDto> FromEntity = entity => new OrderSchedulingListDto
        {
            OrderName = entity.OrderName,
            OrderId = entity.Id,
            WorkingNumber = entity.WorkingNumber,
            CurrentStatus = new OrderStateListDto(entity.CurrentTicket.OrderState),
            EstimatedProcessTime = entity.EstimatedProcessTime,
            Completion = entity.Completion,
            Deadline = entity.Deadline
        };

    }
}

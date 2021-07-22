using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public string CurrentStatus { get; set; }
        public OrderStateEnum CurrentStatusEnum { get; set; }
        public DateTime? StatusDeadline { get; set; }
        public string ResponsibleName { get; set; }
        public string CustomerName { get; set; }
        public string SalesName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets how to map the DBO to this DTO
        /// </summary>
        public static Func<Order, OrderListDto> FromEntity = entity => new OrderListDto
        {
            Id = entity.Id,
            OrderName = entity.OrderName,
            WorkingNumber = entity.WorkingNumber,
            CurrentStatus = entity.CurrentTicket.OrderState.CurrentTranslation.Name,
            CurrentStatusEnum = entity.CurrentTicket.OrderState.State,
            StatusDeadline = entity.CurrentTicket.Deadline,
            ResponsibleName = entity.CurrentTicket.AssignedTo != null ? entity.CurrentTicket.AssignedTo.CurrentVersion.Name : null,
            CustomerName = entity.Customer.User.CurrentVersion.Name,
            SalesName = entity.SalesPerson.User.CurrentVersion.Name,
            CreatedOn = entity.CreationTime,
            Deadline = entity.Deadline
        };
    }
}

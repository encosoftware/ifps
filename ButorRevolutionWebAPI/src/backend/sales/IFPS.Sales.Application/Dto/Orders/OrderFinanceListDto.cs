using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class OrderFinanceListDto
    {
        public Guid Id { get; set; }
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public OrderStateEnum CurrentStatus { get; set; }
        public DateTime? StatusDeadline { get; set; }
        public string CustomerName { get; set; }
        public DateTime Deadline { get; set; }
        public PriceListDto Amount { get; set; }

        public static Expression<Func<Order, OrderFinanceListDto>> Projection
        {
            get
            {
                return x => new OrderFinanceListDto
                {
                    Id = x.Id,
                    OrderName = x.OrderName,
                    WorkingNumber = x.WorkingNumber,
                    CurrentStatus = x.CurrentTicket.OrderState.State,
                    StatusDeadline = x.CurrentTicket.Deadline,
                    CustomerName = x.Customer.User.CurrentVersion.Name,
                    Deadline = x.Deadline,
                };
            }
        }

        public static Func<Order, OrderFinanceListDto> FromEntity = entity => new OrderFinanceListDto
        {
            Id = entity.Id,
            OrderName = entity.OrderName,
            WorkingNumber = entity.WorkingNumber,
            CurrentStatus = entity.CurrentTicket.OrderState.State,
            StatusDeadline = entity.CurrentTicket.Deadline,
            CustomerName = entity.Customer.User.CurrentVersion.Name,
            Deadline = entity.Deadline
        };
    }
}

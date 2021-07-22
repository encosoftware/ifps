using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Domain.Dbos
{
    public class OrdersDbo
    {
        public string OrderId { get; set; }
        public OrderState CurrentStatus { get; set; }
        public DateTime? StatusDeadline { get; set; }
        public User Responsible { get; set; }
        public User Customer { get; set; }
        public User Sales { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Deadline { get; set; }

        public static Expression<Func<Order, OrdersDbo>> Projection
        {
            get
            {
                return x => new OrdersDbo
                {
                    OrderId = x.OrderName,
                    CurrentStatus = x.CurrentTicket.OrderState,
                    Responsible = x.CurrentTicket.AssignedTo ?? null,
                    StatusDeadline = x.CurrentTicket.Deadline,
                    Customer = x.Customer.User,
                    Sales = x.SalesPerson != null ? x.SalesPerson.User : null,
                    CreatedOn = x.CreationTime,
                    Deadline = x.Deadline
                };
            }
        }
    }
}

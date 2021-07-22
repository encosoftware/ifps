using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class TicketListDto
    {
        public Guid OrderId { get; set; }
        public string OrderName { get; set; }
        public OrderStateEnum OrderStateEnum { get; set; }
        public string CustomerName { get; set; }
        public bool IsExpired { get; set; }

        public TicketListDto()
        {
        }

        public TicketListDto(Order order)
        {
            OrderId = order.Id;
            OrderName = order.OrderName;
            OrderStateEnum = order.CurrentTicket.OrderState.State;
            CustomerName = order.Customer.User.CurrentVersion.Name;
            IsExpired = order.CurrentTicket.Deadline < DateTime.Now;
        }

        public static Expression<Func<Order, TicketListDto>> Projection
        {
            get
            {
                return x => new TicketListDto
                {
                    OrderId = x.Id,
                    OrderName = x.OrderName,
                    OrderStateEnum = x.CurrentTicket.OrderState.State,
                    IsExpired = x.CurrentTicket.Deadline < Clock.Now,
                    CustomerName = x.Customer.User.CurrentVersion.Name,
                };
            }
        }
    }
}

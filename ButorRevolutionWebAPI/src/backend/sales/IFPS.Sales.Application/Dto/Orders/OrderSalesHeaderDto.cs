using IFPS.Sales.Domain.Dbos;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class OrderSalesHeaderDto
    {
        public string OrderId { get; set; }
        public string OrderName { get; set; }
        public OrderStateDto CurrentStatus { get; set; }
        public DateTime? StatusDeadline { get; set; }
        public UserDto Responsible { get; set; }
        public UserDto Customer { get; set; }
        public UserDto Sales { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Deadline { get; set; }
        public AddressDetailsDto CustomerAddress { get; set; }
        // Get the user's roles for documents upload by order details (partner can upload partner docs only, etc.)
        public List<DivisionTypeEnum> DivisionTypes { get; set; }

        public static OrderSalesHeaderDto FromModel(Order order, List<DivisionTypeEnum> divisionTypes)
        {
            return new OrderSalesHeaderDto()
            {
                OrderId = order.Id.ToString(),
                OrderName = order.OrderName,
                CurrentStatus = new OrderStateDto(order.CurrentTicket.OrderState),
                StatusDeadline = order.CurrentTicket.Deadline,
                Responsible = new UserDto(order.SalesPerson.User),
                Customer = new UserDto(order.Customer.User),
                Sales = order.SalesPerson != null ? new UserDto(order.SalesPerson.User) : null,
                CreatedOn = order.CreationTime,
                Deadline = order.Deadline,
                CustomerAddress = order.ShippingAddress == null ? new AddressDetailsDto(order.Customer.User.CurrentVersion.ContactAddress) : new AddressDetailsDto(order.ShippingAddress),
                DivisionTypes = divisionTypes
            };
        }
    }
}

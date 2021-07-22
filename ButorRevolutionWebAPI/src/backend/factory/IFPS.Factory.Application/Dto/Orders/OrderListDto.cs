using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class OrderListDto
    {
        public string OrderName { get; set; }

        public string WorkingNumber { get; set; }

        public DateTime Deadline { get; set; }

        public AddressDetailsDto Address { get; set; }

        public OrderStateEnum? StatusType { get; set; }

        public List<ConcreteFurnitureUnitListDto> FurnitureUnits { get; set; }

        public List<ConcreteApplianceMaterialListDto> OrderedApplianceMaterials { get; set; }

        public OrderListDto(Order order)
        {
            OrderName = order.OrderName;
            WorkingNumber = order.WorkingNumber;
            Deadline = order.Deadline;
            Address = new AddressDetailsDto(order.ShippingAddress);
            StatusType = order.CurrentTicket.OrderState?.State;
            FurnitureUnits = order.ConcreteFurnitureUnits.Select(ent => new ConcreteFurnitureUnitListDto(ent)).ToList();
            OrderedApplianceMaterials = order.ConcreteApplianceMaterials.Select(ent => new ConcreteApplianceMaterialListDto(ent)).ToList();
        }
    }
}

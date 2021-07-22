using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceCreateByOfferDto
    {
        public Guid ApplianceMaterialId { get; set; }

        public int Quantity { get; set; }

        public ApplianceCreateByOfferDto()
        {

        }

        public OrderedApplianceMaterial CreateModelObject(Guid orderId)
        {
            return new OrderedApplianceMaterial() { ApplianceMaterialId = ApplianceMaterialId, Quantity = Quantity, OrderId = orderId };
        }
    }
}

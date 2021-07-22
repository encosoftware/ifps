using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitCreateByOfferDto
    {
        public Guid FurnitureUnitId { get; set; }

        public int Quantity { get; set; }

        public FurnitureUnitCreateByOfferDto()
        {

        }

        public OrderedFurnitureUnit CreateModelObject(Guid orderId)
        {
            return new OrderedFurnitureUnit(orderId, FurnitureUnitId, Quantity);
        }

        public OrderedService CreateModelObject(Guid orderId, int serviceId)
        {
            return new OrderedService() { OrderId = orderId, ServiceId = serviceId };
        }
    }
}

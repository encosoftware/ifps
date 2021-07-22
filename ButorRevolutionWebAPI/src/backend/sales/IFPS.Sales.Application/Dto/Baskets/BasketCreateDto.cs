using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class BasketCreateDto
    {
        public int? CustomerId { get; set; }
        public List<OrderedFurnitureUnitCreateDto> OrderedFurnitureUnits { get; set; }
        public DeliveryPriceCreateDto DeliveryPrice { get; set; }
        public int? ServiceId { get; set; }

        public BasketCreateDto() { }

        public Basket CreateModelObject()
        {
            return new Basket(CustomerId) { DelieveryPrice = DeliveryPrice != null ? DeliveryPrice.CreateModelObject() : Price.GetDefaultPrice(), ServiceId = ServiceId ?? 1 };
        }
    }
}

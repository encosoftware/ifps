using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class BasketUpdateDto
    {
        public int? CustomerId { get; set; }
        public List<OrderedFurnitureUnitUpdateDto> OrderedFurnitureUnits { get; set; }
        public PriceCreateDto DeliveryPrice { get; set; }
        public int? ServiceId { get; set; }

        public BasketUpdateDto() { }

        public Basket UpdateModelObject(Basket basket)
        {
            basket.DelieveryPrice = DeliveryPrice != null ? DeliveryPrice.CreateModelObject() : Price.GetDefaultPrice();
            basket.ServiceId = ServiceId ?? 1;
            return basket;
        }
    }
}

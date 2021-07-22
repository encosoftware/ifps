using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class BasketDetailsDto
    {
        public List<OrderedFurnitureUnitListDto> OrderedFurnitureUnits { get; set; }
        public PriceListDto SubTotal { get; set; }
        public PriceListDto DelieveryPrice { get; set; }

        public BasketDetailsDto() { }
        public BasketDetailsDto(Basket basket)
        {
            OrderedFurnitureUnits = basket.OrderedFurnitureUnits.Select(ent => new OrderedFurnitureUnitListDto(ent)).ToList();
            SubTotal = new PriceListDto(basket.SubTotal);
            DelieveryPrice = new PriceListDto(basket.DelieveryPrice);
        }
    }
}

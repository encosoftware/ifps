using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopOrdersDetailsDto
    {
        public List<WebshopOrdersOfuDetailsDto> OrderedFurnitureUnits { get; set; }
        public WebshopOrdersPriceDetailsDto Total { get; set; }

        public WebshopOrdersDetailsDto(WebshopOrder webshopOrder)
        {
            OrderedFurnitureUnits = new List<WebshopOrdersOfuDetailsDto>();
            foreach (var ofu in webshopOrder.OrderedFurnitureUnits)
            {
                var dto = new WebshopOrdersOfuDetailsDto(ofu);
                OrderedFurnitureUnits.Add(dto);
            }
            Total = new WebshopOrdersPriceDetailsDto(webshopOrder.Basket);
        }
    }
}

using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class OrderedFurnitureUnitListDto
    {
        public Guid FurnitureUnitId { get; set; }
        public int Quantity { get; set; }
        public WebshopFurnitureUnitByWebshopListDto WebshopFurnitureUnitListDto { get; set; }

        public OrderedFurnitureUnitListDto(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            FurnitureUnitId = orderedFurnitureUnit.FurnitureUnitId;
            Quantity = orderedFurnitureUnit.Quantity;
        }
    }
}

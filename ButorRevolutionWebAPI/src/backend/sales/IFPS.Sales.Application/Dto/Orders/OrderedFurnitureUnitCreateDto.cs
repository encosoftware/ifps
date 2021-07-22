using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class OrderedFurnitureUnitCreateDto
    {
        public Guid FurnitureUnitId { get; set; }
        public int Quantity { get; set; }

        public OrderedFurnitureUnitCreateDto() { }

        public OrderedFurnitureUnit CreateModelObject()
        {
            return new OrderedFurnitureUnit(FurnitureUnitId, Quantity);
        }
    }
}

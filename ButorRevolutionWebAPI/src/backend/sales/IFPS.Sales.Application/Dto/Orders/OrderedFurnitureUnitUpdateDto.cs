using System;

namespace IFPS.Sales.Application.Dto
{
    public class OrderedFurnitureUnitUpdateDto
    {
        public Guid FurnitureUnitId { get; set; }
        public int Quantity { get; set; }
    }
}

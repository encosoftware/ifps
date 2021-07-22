using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitUpdateDto
    {
        public Guid FurnitureUnitId { get; set; }
        public List<ImageCreateDto> Images { get; set; }
        public PriceCreateDto Price { get; set; }
    }
}

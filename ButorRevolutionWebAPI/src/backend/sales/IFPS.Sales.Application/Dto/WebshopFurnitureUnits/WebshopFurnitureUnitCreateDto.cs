using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitCreateDto
    {
        public Guid FurnitureUnitId { get; set; }
        public List<ImageCreateDto> Images { get; set; }
        public PriceCreateDto Price { get; set; }

        public WebshopFurnitureUnit CreateModelObject()
        {
            return new WebshopFurnitureUnit(FurnitureUnitId) { Price = Price.CreateModelObject() };
        }
    }
}

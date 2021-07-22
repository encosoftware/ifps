using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitListDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid FurnitureUnitId { get; set; }
        public PriceListDto Price { get; set; }

        public WebshopFurnitureUnitListDto()
        {

        }

        public static Func<WebshopFurnitureUnit, WebshopFurnitureUnitListDto> FromEntity = entity => new WebshopFurnitureUnitListDto
        {
            Id = entity.Id,
            Code = entity.FurnitureUnit.Code,
            Description = entity.FurnitureUnit.Description,
            FurnitureUnitId = entity.FurnitureUnitId,
            Price = new PriceListDto(entity.Price)
        };
    }
}

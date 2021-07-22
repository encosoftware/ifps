using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitListByWebshopCategoryDto
    {
        public int WebshopFurnitureUnitId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public int? CategoryId { get; set; }
        public ImageDetailsDto Image { get; set; }
        public PriceListDto Price { get; set; }

        public WebshopFurnitureUnitListByWebshopCategoryDto(WebshopFurnitureUnit wfu)
        {
            WebshopFurnitureUnitId = wfu.Id;
            Code = wfu.FurnitureUnit.Code;
            Description = wfu.FurnitureUnit.Description;
            Width = wfu.FurnitureUnit.Width;
            Height = wfu.FurnitureUnit.Height;
            Depth = wfu.FurnitureUnit.Depth;
            CategoryId = wfu.FurnitureUnit.CategoryId;
            Image = new ImageDetailsDto(wfu.FurnitureUnit.Image);
            Price = new PriceListDto(wfu.Price);
        }

        public WebshopFurnitureUnitListByWebshopCategoryDto() { }

        public static Func<WebshopFurnitureUnit, WebshopFurnitureUnitListByWebshopCategoryDto> FromEntity = entity => new WebshopFurnitureUnitListByWebshopCategoryDto
        {
            WebshopFurnitureUnitId = entity.Id,
            Code = entity.FurnitureUnit.Code,
            Description = entity.FurnitureUnit.Description,
            Width = entity.FurnitureUnit.Width,
            Height = entity.FurnitureUnit.Height,
            Depth = entity.FurnitureUnit.Depth,
            CategoryId = entity.FurnitureUnit.CategoryId,
            Image = new ImageDetailsDto(entity.FurnitureUnit.Image),
            Price = new PriceListDto(entity.Price)
        };
    }
}

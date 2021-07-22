using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitListDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public CategoryListDto Category { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public PriceListDto SellPrice { get; set; }
        public PriceListDto MaterialCost { get; set; }
        public ImageThumbnailListDto ImageThumbnail { get; set; }

        public FurnitureUnitListDto() { }

        public FurnitureUnitListDto(FurnitureUnit furnitureUnit)
        {
            Id = furnitureUnit.Id;
            Code = furnitureUnit.Code;
            Description = furnitureUnit.Description;
            Category = new CategoryListDto(furnitureUnit.Category);
            Width = furnitureUnit.Width;
            Height = furnitureUnit.Height;
            Depth = furnitureUnit.Depth;
            SellPrice = new PriceListDto(furnitureUnit.CurrentPrice.Price);
            MaterialCost = new PriceListDto(furnitureUnit.CurrentPrice.MaterialCost);
            ImageThumbnail = new ImageThumbnailListDto(furnitureUnit.Image);
        }

        public static Func<FurnitureUnit, FurnitureUnitListDto> FromEntity = entity => new FurnitureUnitListDto(entity);
    }
}

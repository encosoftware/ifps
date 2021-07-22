using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitDetailsByWebshopFurnitureUnitDto
    {
        public Guid FurnitureUnitId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public ImageDetailsDto Image { get; set; }
        public PriceListDto Price { get; set; }

        public FurnitureUnitDetailsByWebshopFurnitureUnitDto(FurnitureUnit furnitureUnit)
        {
            FurnitureUnitId = furnitureUnit.Id;
            Description = furnitureUnit.Description;
            Name = furnitureUnit.Code;
            Width = furnitureUnit.Width;
            Height = furnitureUnit.Height;
            Depth = furnitureUnit.Depth;
            Image = new ImageDetailsDto(furnitureUnit.Image);
            Price = new PriceListDto(furnitureUnit.CurrentPrice.Price);
        }
    }
}

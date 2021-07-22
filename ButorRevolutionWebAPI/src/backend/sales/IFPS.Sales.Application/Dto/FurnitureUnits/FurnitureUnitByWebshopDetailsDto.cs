using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitByWebshopDetailsDto
    {
        public int WebshopFurnitureUnitId { get; set; }

        public Guid FurnitureUnitId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Depth { get; set; }

        public PriceListDto Price { get; set; }

        public ImageDetailsDto Image { get; set; }

        public FurnitureUnitByWebshopDetailsDto()
        {

        }

        public FurnitureUnitByWebshopDetailsDto(WebshopFurnitureUnit webshopFurnitureUnit)
        {
            WebshopFurnitureUnitId = webshopFurnitureUnit.Id;
            FurnitureUnitId = webshopFurnitureUnit.FurnitureUnitId;
            Name = webshopFurnitureUnit.FurnitureUnit.Code;
            Description = webshopFurnitureUnit.FurnitureUnit.Description;
            Width = webshopFurnitureUnit.FurnitureUnit.Width;
            Height = webshopFurnitureUnit.FurnitureUnit.Height;
            Depth = webshopFurnitureUnit.FurnitureUnit.Depth;
            Price = new PriceListDto(webshopFurnitureUnit.Price);
            Image = new ImageDetailsDto(webshopFurnitureUnit.FurnitureUnit.Image);
        }
    }
}

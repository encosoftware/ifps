using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitByWebshopListDto
    {
        public Guid FurnitureUnitId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public CategoryListDto Category { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public PriceListDto SellPrice { get; set; }
        public ImageThumbnailListDto ImageThumbnail { get; set; }

        public WebshopFurnitureUnitByWebshopListDto(WebshopFurnitureUnit wfu)
        {
            FurnitureUnitId = wfu.FurnitureUnit.Id;
            Code = wfu.FurnitureUnit.Code;
            Description = wfu.FurnitureUnit.Description;
            Category = new CategoryListDto(wfu.FurnitureUnit.Category);
            Width = wfu.FurnitureUnit.Width;
            Height = wfu.FurnitureUnit.Height;
            Depth = wfu.FurnitureUnit.Depth;
            SellPrice = new PriceListDto(wfu.Price);
            ImageThumbnail = new ImageThumbnailListDto(wfu.FurnitureUnit.Image);
        }
    }
}

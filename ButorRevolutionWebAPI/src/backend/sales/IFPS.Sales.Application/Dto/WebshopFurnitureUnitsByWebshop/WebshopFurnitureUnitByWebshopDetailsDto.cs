using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitByWebshopDetailsDto
    {
        public Guid FurnitureUnitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public PriceListDto Price { get; set; }
        public List<ImageDetailsDto> Images { get; set; }

        public WebshopFurnitureUnitByWebshopDetailsDto(WebshopFurnitureUnit wfu)
        {
            FurnitureUnitId = wfu.FurnitureUnitId;
            Name = wfu.FurnitureUnit.Code;
            Description = wfu.FurnitureUnit.Description;
            Width = wfu.FurnitureUnit.Width;
            Height = wfu.FurnitureUnit.Height;
            Depth = wfu.FurnitureUnit.Depth;
            Price = new PriceListDto(wfu.Price);
            SetImages(wfu);
        }

        private void SetImages(WebshopFurnitureUnit wfu)
        {
            Images = new List<ImageDetailsDto>();
            Images.AddRange(wfu.Images.Select(ent => new ImageDetailsDto(ent.Image)).ToList());
        }
    }
}

using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitDetailsDto
    {
        public Guid FurnitureUnitId { get; set; }
        public List<ImageDetailsDto> Images { get; set; }
        public PriceListDto Price { get; set; }

        public WebshopFurnitureUnitDetailsDto(WebshopFurnitureUnit wfu)
        {
            FurnitureUnitId = wfu.FurnitureUnitId;
            Price = new PriceListDto(wfu.Price);
            if (wfu.Images.Count() > 0)
            {
                SetImages(wfu);
            }
        }

        private void SetImages(WebshopFurnitureUnit wfu)
        {
            Images = new List<ImageDetailsDto>();
            Images.AddRange(wfu.Images.Select(ent => new ImageDetailsDto(ent.Image)).ToList());
        }
    }
}

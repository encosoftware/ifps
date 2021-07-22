using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitDetailsByOfferDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double Width { get; set; }
        public double Depth { get; set; }
        public double Height { get; set; }
        public int Quantity { get; set; }
        public CategoryDetailsDto Category { get; set; }
        public PriceListDto Total { get; set; }
        public ImageDetailsDto Image { get; set; }
        public List<FurnitureComponentsDetailsByOfferDto> Fronts { get; set; }
        public List<FurnitureComponentsDetailsByOfferDto> Corpuses { get; set; }

        public FurnitureUnitDetailsByOfferDto(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            Code = orderedFurnitureUnit.FurnitureUnit.Code;
            Description = orderedFurnitureUnit.FurnitureUnit.Description;
            Width = orderedFurnitureUnit.FurnitureUnit.Width;
            Depth = orderedFurnitureUnit.FurnitureUnit.Depth;
            Height = orderedFurnitureUnit.FurnitureUnit.Height;
            Quantity = orderedFurnitureUnit.Quantity;
            Category = new CategoryDetailsDto(orderedFurnitureUnit.FurnitureUnit.Category);
            Total = new PriceListDto(orderedFurnitureUnit.FurnitureUnit.CurrentPrice.Price);
            Image = new ImageDetailsDto(orderedFurnitureUnit.FurnitureUnit.Image);
            Fronts = orderedFurnitureUnit.FurnitureUnit.Components.Where(ent => ent.Type == FurnitureComponentTypeEnum.Front).Select(ent => new FurnitureComponentsDetailsByOfferDto(ent)).ToList();
            Corpuses = orderedFurnitureUnit.FurnitureUnit.Components.Where(ent => ent.Type == FurnitureComponentTypeEnum.Corpus).Select(ent => new FurnitureComponentsDetailsByOfferDto(ent)).ToList();
        }
    }
}

using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitCreateWithQuantityByOfferDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public List<FurnitureComponentsCreateByOfferDto> Fronts { get; set; }
        public List<FurnitureComponentsCreateByOfferDto> Corpuses { get; set; }

        public FurnitureUnitCreateWithQuantityByOfferDto()
        {

        }

        public FurnitureUnit CreateModelObject(Guid baseFurnitureUnitId, Guid imageId, int? currentPriceId, int furnitureUnitTypeId)
        {
            var furnitureUnit = new FurnitureUnit(Code, Width, Height, Depth)
            {
                Description = Description,
                CategoryId = CategoryId,
                BaseFurnitureUnitId = baseFurnitureUnitId,
                ImageId = imageId,
                CurrentPriceId = currentPriceId,
                FurnitureUnitTypeId = furnitureUnitTypeId
            };

            return furnitureUnit;
        }
    }
}

using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitInBasketIdsDto
    {
        public List<Guid> FurnitureUnitIds { get; set; }
        public int RecommendedItemNum { get; set; }
    }
}

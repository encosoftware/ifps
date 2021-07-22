using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitFilterByWebshopCategoryDto : OrderedPagedRequestDto
    {
        public FurnitureUnitTypeEnum UnitType { get; set; }
        public double MinimumPrice { get; set; }
        public double MaximumPrice { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(MinimumPrice), nameof(WebshopFurnitureUnit.Value) }
            };
        }
    }
}

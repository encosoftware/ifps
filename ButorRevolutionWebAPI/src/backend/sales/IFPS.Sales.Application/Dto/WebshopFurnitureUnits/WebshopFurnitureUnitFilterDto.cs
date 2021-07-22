using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitFilterDto : OrderedPagedRequestDto
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public static Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                { nameof(Code), nameof(WebshopFurnitureUnit.FurnitureUnit.Code) },
                { nameof(Description), nameof(WebshopFurnitureUnit.FurnitureUnit.Description) }
            };
        }
    }
}

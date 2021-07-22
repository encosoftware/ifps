using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryMaterialFilterDto : OrderedPagedRequestDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsOptional { get; set; }
        public bool? IsRequiredForAssembly { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
            {
                { nameof(Code), nameof(AccessoryMaterial.Code) },
                { nameof(Description), nameof(AccessoryMaterial.Description) },
                { nameof(CategoryId), nameof(AccessoryMaterial.CategoryId) }
            };
        }
    }
}

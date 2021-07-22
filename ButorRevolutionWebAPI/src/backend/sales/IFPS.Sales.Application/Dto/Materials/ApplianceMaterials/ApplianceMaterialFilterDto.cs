using ENCO.DDD.Application.Dto;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class ApplianceMaterialFilterDto : OrderedPagedRequestDto
    {
        public string Brand { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string HanaCode { get; set; }

        public static Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                { nameof(Brand), nameof(ApplianceMaterial.Brand) },
                { nameof(Description), nameof(ApplianceMaterial.Description) },
                { nameof(Code), nameof(ApplianceMaterial.Code) },
                { nameof(CategoryId), nameof(ApplianceMaterial.CategoryId) },
                { nameof(HanaCode), nameof(ApplianceMaterial.HanaCode) }
            };
        }
    }
}

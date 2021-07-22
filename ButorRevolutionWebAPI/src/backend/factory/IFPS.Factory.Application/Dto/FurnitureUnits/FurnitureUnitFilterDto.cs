using ENCO.DDD.Application.Dto;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureUnitFilterDto : OrderedPagedRequestDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public bool? IsUploaded { get; set; }

        public static Dictionary<string, string> GetOrderingMapping()
        {
            return new Dictionary<string, string>
                {
                    { nameof(Code), nameof(FurnitureUnit.Code) },
                    { nameof(Description), nameof(FurnitureUnit.Description) },
                    { nameof(CategoryId), nameof(FurnitureUnit.CategoryId) }
              };
        }
    }
}
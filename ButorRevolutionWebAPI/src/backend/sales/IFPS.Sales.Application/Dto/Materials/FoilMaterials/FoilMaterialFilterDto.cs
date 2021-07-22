using ENCO.DDD.Application.Dto;

namespace IFPS.Sales.Application.Dto
{
    public class FoilMaterialFilterDto : OrderedPagedRequestDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
